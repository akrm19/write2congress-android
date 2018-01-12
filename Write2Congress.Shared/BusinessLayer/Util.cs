using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.ApiModels.ProPublica;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;
using static Write2Congress.Shared.DomainModel.ApiModels.ProPublica.CongressMembersResult;
using static Write2Congress.Shared.DomainModel.ApiModels.ProPublica.SenateMembersResult;

namespace Write2Congress.Shared.BusinessLayer
{
    public class Util
    {
        private IMyLogger _logger;
        public Util(IMyLogger logger)
        {
            _logger = logger;
        }

        #region App Agnostic

        //TODO RM: This also exist in extensions. Pick one.
        public static T DeserializeFromJson<T>(string jsonSerializedContent)
        {
            if (string.IsNullOrWhiteSpace(jsonSerializedContent))
                throw new JsonSerializationException($"Cannot deserialize {typeof(T).ToString()} from an empty string");

            return JsonConvert.DeserializeObject<T>(jsonSerializedContent);
        }

        public static List<T> GetJsonSerializedObjsFromFile<T>(string path, string extension = "", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            var result = new List<T>();

            var letterFiles = Util.GetAllFilesInDir
                (path, $"*.{extension}", SearchOption.AllDirectories);

            foreach (var letterFilePath in letterFiles)
            {
                if (!File.Exists(letterFilePath))
                    continue;

                var letterContent = File.ReadAllText(letterFilePath);

                if (string.IsNullOrWhiteSpace(letterContent))
                    continue;

                var letter = DeserializeFromJson<T>(letterContent);

                result.Add(letter);
            }

            return result;
        }

        public static string GetFileContents(string filePath)
        {
            if (File.Exists(filePath))
                return File.ReadAllText(filePath);

            //TOD RM: Add logging
            //_logger.Error($"File does not exist, cannot retrieve file contents. Filepath: {filePath}");
            return string.Empty;
        }

        public static bool DeleteFile(string filePath)
        {

            if (!File.Exists(filePath))
                return true;

            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //TODO RM: Add loggin
                //
            }
        }

        public static bool CreateFileContent(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                // TODO RM: Add logging
                //_logger.Error($"Cannot write file content to file ({filePath}). Error: {ex.Message}");
                return false;
            }
        }

        public static string[] GetAllFilesInDir(string dirPath, string pattern = "", SearchOption searOptions = SearchOption.TopDirectoryOnly)
        {
            if (!Directory.Exists(dirPath))
            {

                //TODO RM: Add logging
                //_logger.Error($"Directory does not exist. Cannot retrive files, returning empty array. Directory: {dirPath}");
                return new string[] { };
            }

            var dir = new DirectoryInfo(dirPath);

            return string.IsNullOrWhiteSpace(pattern)
                ? Directory.GetFiles(dirPath)
                : Directory.GetFiles(dirPath, pattern, searOptions);
        }

        public static void CreateDir(string dirPath)
        {
            if (Directory.Exists(dirPath))
                return;

            try
            {
                Directory.CreateDirectory(dirPath);
            }
            catch (Exception ex)
            {
                //TODO RM Add logging
                //_logger.Error($"Error creating directory: {dirPath}. {ex.Message}");
            }
        }

        #endregion

        #region App Specific Helper Methods

        public static Legislator DeserializeLegislatorJson(Legislator objectToSerialize, string jsonSerializedContent)
        {
            if (string.IsNullOrWhiteSpace(jsonSerializedContent))
                throw new JsonSerializationException($"Cannot deserialize {objectToSerialize.GetType().ToString()} from an empty string");

            return JsonConvert.DeserializeObject<Legislator>(jsonSerializedContent);
        }

        public static StateOrTerritory GetStateOrTerrByDescription(string stateOrTerrDescription, StateOrTerritory defaultStateOrTerritory)
        {
            var statesOrTerrWithDescription = GetAllStatesAndTerrWithDescriptions();
            var stateOrTerrDescriptionLowerCase = stateOrTerrDescription.ToLower();

            Tuple<StateOrTerritory, string> matchingStateOrTerrSet = statesOrTerrWithDescription
                .Where(set => set.Item2.ToLower().Equals(stateOrTerrDescriptionLowerCase))
                .FirstOrDefault();

            if (matchingStateOrTerrSet != null)
                return matchingStateOrTerrSet.Item1;

            //_logger.Error($"Could not parse StateOrTerritory: {stateOrTerrDescription}. Returning default value {defaultStateOrTerritory}");
            return defaultStateOrTerritory;
        }

        public static StateOrTerritory GetStateOrTerrByName(string stateOrTerrName, StateOrTerritory defaultStateOrTerritory)
        {
            StateOrTerritory stateOrTerritory;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerrName, out stateOrTerritory))
                return stateOrTerritory;

            //_logger.Error($"Could not parse StateOrTerritory: {stateOrTerrName}. Returning default value {defaultStateOrTerritory.ToString()}");
            return defaultStateOrTerritory;
        }

        public static List<Tuple<StateOrTerritory, string>> GetAllStatesAndTerrWithDescriptions()
        {
            var result = new List<Tuple<StateOrTerritory, string>>();

            foreach (var stateOrTerr in Enum.GetValues(typeof(StateOrTerritory)))
            {
                var stateOrTerrEnum = (StateOrTerritory)stateOrTerr;
                var description = stateOrTerrEnum.GetDescription();

                var newSet = new Tuple<StateOrTerritory, string>(stateOrTerrEnum, description ?? string.Empty);
                result.Add(newSet);
            }

            return result;
        }

        public static string GetUrlFromSocialContactMethod(ContactMethod contactMethod)
        {
            switch (contactMethod.Type)
            {
                case ContactType.NotSet:
                    return string.Empty;
                case ContactType.Facebook:
                    return $"http://facebook.com/{contactMethod.ContactInfo}";
                case ContactType.Twitter:
                    return $"http://twitter.com/{contactMethod.ContactInfo}";
                case ContactType.YouTube:
                    return $"http://youtube.com/{contactMethod.ContactInfo}";
                case ContactType.WebSite:
                    return contactMethod.ContactInfo;
                case ContactType.WebSiteContact:
                    return contactMethod.ContactInfo;
                default:
                    return string.Empty;
            }
        }

        public bool ValidZipFormat(string zip)
        {
            if (zip.Length != 5)
                return false; //TODO Add logging

            int zipNumber = 0;

            //TODO RM: handle errors, logging or exception
            return int.TryParse(zip, out zipNumber);            
        }
        #endregion

        #region Sunlight Api Helper Methods
        public static List<Legislator> LegislatorsFromSunlightLegislatorResult(SunlightLegislatorResult.Rootobject legislatorResults)
        {
            var legislators = new List<Legislator>();

            foreach (var l in legislatorResults.results)
            {
                var legislator = new Legislator()
                {
                    FirstName = l.first_name ?? string.Empty,
                    MiddleName = l.middle_name ?? string.Empty,
                    LastName = l.last_name ?? string.Empty,
                    Birthday = DataTransformationUtil.DateFromSunlightTime(l.birthday),
                    Party = DataTransformationUtil.PartyFromString(l.party),
                    Chamber = DataTransformationUtil.LegislativeBodyFromSunlight(l.chamber),
                    State = DataTransformationUtil.GetStateOrTerritoryFromSunlight(l.state),
                    Gender = DataTransformationUtil.GenderFromString(l.gender),
                    TermStartDate = DataTransformationUtil.DateFromSunlightTime(l.term_start),
                    TermEndDate = DataTransformationUtil.DateFromSunlightTime(l.term_end),
                    BioguideId = l.bioguide_id ?? string.Empty,

                    OfficeAddress = string.IsNullOrWhiteSpace(l.office)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Mail, l.office),
                    OfficeNumber = string.IsNullOrWhiteSpace(l.phone)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Phone, l.phone),
                    Email = string.IsNullOrWhiteSpace(l.oc_email)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Email, l.oc_email),

                    FacebookId = string.IsNullOrWhiteSpace(l.facebook_id)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Facebook, l.facebook_id),
                    TwitterId = string.IsNullOrWhiteSpace(l.twitter_id)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Twitter, l.twitter_id),
                    YouTubeId = string.IsNullOrWhiteSpace(l.youtube_id)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.YouTube, l.youtube_id),
                    Website = string.IsNullOrWhiteSpace(l.website)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.WebSite, l.website),
                    ContactSite = string.IsNullOrWhiteSpace(l.contact_form)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.WebSiteContact, l.contact_form),


                    TotalVotes = 0, 
                    MissedVotesPercent = 0,
                    VotesWithPartyPercent = 0,
                    Senority = string.Empty
                };

                legislators.Add(legislator);
            }

            return legislators;
        }

        //public static List<Legislator> LegislatorsFromPropublicaLegislatorsResult(BaseLegislatorsResult.Rootobject legislatorResults)
        public static List<Legislator> LegislatorsFromPropublicaLegislatorsResult(SenateMembersResult.Rootobject legislatorResults)
        {
            var legislators = new List<Legislator>();

            var results = legislatorResults.results.FirstOrDefault();
            if (results == null)
                return legislators;

            foreach (var l in results.members)
            {
                var legislator = new Legislator()
                {
                    FirstName = l.first_name ?? string.Empty,
                    MiddleName = l.middle_name ?? string.Empty,
                    LastName = l.last_name ?? string.Empty,
                    Birthday = DataTransformationUtil.DateFromSunlightTime(l.date_of_birth),
                    Party = DataTransformationUtil.PartyFromString(l.party),
                    State = DataTransformationUtil.GetStateOrTerritoryFromSunlight(l.state),
                    OfficeAddress = string.IsNullOrWhiteSpace(l.office)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Mail, l.office),
                    OfficeNumber = string.IsNullOrWhiteSpace(l.phone)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Phone, l.phone),
                    FacebookId = string.IsNullOrWhiteSpace(l.facebook_account)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Facebook, l.facebook_account),
                    TwitterId = string.IsNullOrWhiteSpace(l.twitter_account)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Twitter, l.twitter_account),
                    YouTubeId = string.IsNullOrWhiteSpace(l.youtube_account)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.YouTube, l.youtube_account),
                    Website = string.IsNullOrWhiteSpace(l.url)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.WebSite, l.url),
                    ContactSite = string.IsNullOrWhiteSpace(l.contact_form)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.WebSiteContact, l.contact_form),
                    BioguideId = l.id ?? string.Empty, 


                    //TODO RM: These do not exist in new ProPublica source
                    Gender = Gender.NA,
                    TermStartDate = DateTime.MinValue,
                    TermEndDate = DateTime.MinValue,
                    Email = new ContactMethod(ContactType.NotSet, string.Empty),

                             
                    //TODO Verify adn add checking for these values
                    TotalVotes = l.total_votes ?? 0,
                    MissedVotesPercent = l.missed_votes_pct ?? 0,
                    VotesWithPartyPercent = l.votes_with_party_pct ?? 0,
                    Senority = l.seniority ?? string.Empty
                };
                
                //TODO RM: Newlsy added, verify this works
                //TODO FIX: type will be root since it is serialized as such.
                var typeOfMember = l.GetType();
                if (typeOfMember == typeof(SenateMember))
                    legislator.Chamber = LegislativeBody.Senate;
                else if (typeOfMember == typeof(CongressMember))
                    legislator.Chamber = LegislativeBody.House;
                else
                    legislator.Chamber = LegislativeBody.Unknown;

                legislators.Add(legislator);
            }

            return legislators;
        }

        public static List<Committee> CommitteesFromSunlightCommitteeResult(SunlightCommitteeResult.Rootobject committeeResults)
        {
            var committees = new List<Committee>();

            foreach (var c in committeeResults.results)
            {
                var committee = new Committee()
                {
                    Id = c.committee_id ?? string.Empty,
                    Name = c.name ?? string.Empty,
                    Chamber = DataTransformationUtil.LegislativeBodyFromSunlight(c.chamber),
                    IsSubcommittee = c.subcommittee,
                    ParentCommitteeId = c.subcommittee
                        ? (c.parent_committee_id ?? string.Empty)
                        : string.Empty,
                    Phone = c.phone ?? string.Empty,
                    Url = c.url ?? string.Empty
                };

                committees.Add(committee);
            }

            return committees;
        }

        public List<Vote> VotesFromSunlightVoteResult(SunlightVoteResult.Rootobject result, string legislatorBioguideId)
        {
            var votes = new List<Vote>();

            foreach (var v in result.results)
            {
                Vote vote = VoteFromSunlightVote(v, legislatorBioguideId);

                if (vote != null)
                    votes.Add(vote);
            }

            return votes;
        }

        private Vote VoteFromSunlightVote(SunlightVoteResult.SunlightVote v, string legislatorBioguideId)
        {
            try
            {
                var vote = new Vote()
                {
                    Bill = v.bill == null
                        ? null
                        : BillFromSunlightBill(v.bill),
                    BillId = v.bill_id ?? string.Empty,
                    Chamber = DataTransformationUtil.LegislativeBodyFromSunlight(v.chamber),
                    Question = v.question ?? string.Empty,
                    Result = v.result,
                    Source = v.source,
                    Type = new VoteType( v.vote_type ?? string.Empty,
                        DataTransformationUtil.VoteTypeKindFromSunlightVoteType(v.vote_type)),
                    VotedAt = DataTransformationUtil.DateFromSunlightTime(v.voted_at),
                    Year = v.year ?? 0,
                    VoteCastedByLegislator = DataTransformationUtil.VoteCasedTypeFromSunlight(v.voter_ids, legislatorBioguideId),
                    NominationId = v.nomination_id ?? string.Empty,
                    Nomination = NominationFromSunlightNomination(v.nomination)
                };

                return vote;
            }
            catch (Exception ex)
            {
                _logger.Error("Error: Could not create Vote from Sunlight vote", ex);
                return null;
            }
        }

        private Nomination NominationFromSunlightNomination(SunlightVoteResult.SunlighNomination n)
        {
            if (n == null)
                return null;

            var nomination = new Nomination()
            {
                DateOfLastAction = DataTransformationUtil.DateFromSunlightTime(n.last_action_at),
                DateReceived = DataTransformationUtil.DateFromSunlightTime(n.received_on),
                Organization = n.organization ?? string.Empty,
                Nominees = NomineesFromSunlightNominees(n.nominees)
            };

            return nomination;
        }

        private List<Nominee> NomineesFromSunlightNominees(SunlightVoteResult.SunlightNominee[] sunlightNominees)
        {
            var nominees = new List<Nominee>();

            if (sunlightNominees == null)
            {
                _logger.Warn("Cannot retrive nominees from SunlightNominees because the value is null!");
                return nominees;
            }

            foreach(var n in sunlightNominees)
            {
                var nominee = new Nominee(n.name ?? string.Empty, n.position ?? string.Empty, n.state);

                nominees.Add(nominee);
            }

            return nominees;
        }



        public List<Bill> BillsFromSunlightBillResult(SunlightBillResult.Rootobject billResults)
        {
            var bills = new List<Bill>();

            foreach(var b in billResults.results)
            {
                var bill = BillFromSunlightBill(b);

                if(bill != null)
                    bills.Add(bill);
            }

            return bills;
        }

        private Bill BillFromSunlightBill(SunlightBillResult.SunlightBill b)
        {
            try
            {
                var bill = new Bill()
                {
                    Chamber = DataTransformationUtil.LegislativeBodyFromSunlight(b.chamber),
                    Congress = b.congress ?? 0,
                    CosponsorIds = b.cosponsor_ids ?? new string[0],
                    DateIntroduced = DataTransformationUtil.DateFromSunlightTime(b.introduced_on),
                    DateOfLastVote = DataTransformationUtil.DateFromSunlightTime(b.last_vote_at),
                    History = HistoryFromSunlight(b.history),
                    Id = b.bill_id ?? string.Empty,
                    LastAction = ActionFromSunlight(b.last_action),
                    Nicknames = b.Nicknames ?? new string[0],
                    Number = b.number ?? 0,
                    SponsorId = b.sponsor_id ?? string.Empty,
                    Summary = b.summary ?? string.Empty,
                    SummaryCappedAt1k = b.summary_short ?? string.Empty,
                    Titles = new BillTitles()
                    {
                        OfficialTile = b.official_title ?? string.Empty,
                        PopularTitlePerLoc = b.popular_title ?? string.Empty,
                        ShortTitle = b.short_title ?? string.Empty
                    },
                    Type = BillTypeFromSunlight(b.bill_type),
                    UpcomingActions = UpcomingBillActionFromSunlight(b.upcoming),
                    Urls = UrlsFromSunlightBillUrls(b.urls),
                    WithdrawnCosponsorIds = b.withdrawn_cosponsor_ids == null
                        ? new List<string>()
                        : new List<string>(b.withdrawn_cosponsor_ids)
                };

                return bill;
            }
            catch (Exception ex)
            {
                _logger.Error("Error encoutnered creating Bill from SunlightBill.", ex);
                return null;
            }
        }

        private static List<string> UrlsFromSunlightBillUrls(SunlightBillResult.Urls urls)
        {
            var result = new List<string>();

            if (urls == null)
                return result;

            if (!string.IsNullOrWhiteSpace(urls.congress))
                result.Add(urls.congress);

            if (!string.IsNullOrWhiteSpace(urls.govtrack))
                result.Add(urls.govtrack);

            return result;
        }

        private static List<UpcomingAction> UpcomingBillActionFromSunlight(SunlightBillResult.Upcoming[] upcoming)
        {
            var upcomingActions = new List<UpcomingAction>();

            if (upcoming == null)
                return upcomingActions;

            foreach (var ua in upcoming)
            {
                var upcomingAction = new UpcomingAction()
                {
                    Chamber = DataTransformationUtil.LegislativeBodyFromSunlight(ua.chamber),
                    Context = ua.context ?? string.Empty,
                    Date = DataTransformationUtil.DateFromSunlightTime(ua.scheduled_at),
                    Url = ua.url ?? string.Empty
                };

                upcomingActions.Add(upcomingAction);
            }

            return upcomingActions;
        }

        /// <summary>
        ///  The type for this bill. For the bill “H.R. 4921”, the bill_type represents the 
        /// “H.R.” part. Bill types can be: hr, hres, hjres, hconres, s, sres, sjres, sconres.
        /// </summary>
        /// <param name="bill_type"></param>
        /// <returns></returns>
        private static BillType BillTypeFromSunlight(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return new BillType(BillTypeKind.Empty, string.Empty);

            var billTypeKind = BillTypeKind.Empty;

            switch (type.ToLower())
            {
                case "hr":
                    billTypeKind = BillTypeKind.hr;
                    break;
                case "hres":
                    billTypeKind = BillTypeKind.hres;
                    break;
                case "hjres":
                    billTypeKind = BillTypeKind.hjres;
                    break;
                case "hconres":
                    billTypeKind = BillTypeKind.hconres;
                    break;
                case "s":
                    billTypeKind = BillTypeKind.s;
                    break;
                case "sres":
                    billTypeKind = BillTypeKind.sres;
                    break;
                case "sjres":
                    billTypeKind = BillTypeKind.sjres;
                    break;
                case "sconres":
                    billTypeKind = BillTypeKind.sconres;
                    break;
                default:
                    break;
            }

            return new BillType(billTypeKind, type);
        }

        private static BillAction ActionFromSunlight(SunlightBillResult.Action action)
        {
            if (action == null)
                return null;

            var billAction = new BillAction()
            {
                Date = DataTransformationUtil.DateFromSunlightTime(action.acted_at),
                Text = action.text ?? string.Empty,
                Type = DataTransformationUtil.BillActionTypeFromSunlight(action.type)
            };

            return billAction;
        }



        private static BillHistory HistoryFromSunlight(SunlightBillResult.History billHistory)
        {
            if (billHistory == null)
                return null;

            var history = new BillHistory()
            {
                AwaitingSignature = billHistory.awaiting_signature,
                AwaitingSignatureSince = DataTransformationUtil.DateFromSunlightTime(billHistory.awaiting_signature_since),
                DateHouseLastVotedOnPassage = DataTransformationUtil.DateFromSunlightTime(billHistory.house_passage_result_at),
                DateSenateLastVotedOnPassage = DataTransformationUtil.DateFromSunlightTime(billHistory.senate_passage_result_at),
                DateEnacted = DataTransformationUtil.DateFromSunlightTime(billHistory.enacted_at),
                Enacted = billHistory.enacted,
                DateVetoed = DataTransformationUtil.DateFromSunlightTime(billHistory.vetoed_at),
                Vetoed = billHistory.vetoed,
                HousePassageResult = DataTransformationUtil.LegislativeBillVoteFromSunlight(billHistory.house_passage_result),
                SenatePassageResult = DataTransformationUtil.LegislativeBillVoteFromSunlight(billHistory.senate_passage_result)
            };

            return history;
        }




        #endregion
    }

    public static class TransformationExtension
    {
        public static List<Legislator> LegislatorsFromPropublicaLegislatorsResult(this SenateMembersResult.Rootobject legislatorResults)
        {
            var legislators = new List<Legislator>();

            var results = legislatorResults.results.FirstOrDefault();
            if (results == null)
                return legislators;

            foreach (var l in results.members)
            {
                var legislator = LegislatorFromPropublicCongressApi(l);
                legislator.Chamber = LegislativeBody.Senate;

                legislators.Add(legislator);
            }

            return legislators;
        }

        public static List<Legislator> LegislatorsFromPropublicaLegislatorsResult(this CongressMembersResult.Rootobject legislatorResults)
        {
            var legislators = new List<Legislator>();

            var results = legislatorResults.results.FirstOrDefault();
            if (results == null)
                return legislators;

            foreach (var l in results.members)
            {
                var legislator = LegislatorFromPropublicCongressApi(l);
                legislator.Chamber = LegislativeBody.House;

                legislators.Add(legislator);
            }

            return legislators;
        }

        private static Legislator LegislatorFromPropublicCongressApi(BaseLegislatorsResult.Member l)
        {
            var legislator = new Legislator()
            {
                FirstName = l.first_name ?? string.Empty,
                MiddleName = l.middle_name ?? string.Empty,
                LastName = l.last_name ?? string.Empty,
                Birthday = DataTransformationUtil.DateFromSunlightTime(l.date_of_birth),
                Party = DataTransformationUtil.PartyFromString(l.party),
                State = DataTransformationUtil.GetStateOrTerritoryFromSunlight(l.state),
                OfficeAddress = string.IsNullOrWhiteSpace(l.office)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.Mail, l.office),
                OfficeNumber = string.IsNullOrWhiteSpace(l.phone)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.Phone, l.phone),
                FacebookId = string.IsNullOrWhiteSpace(l.facebook_account)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.Facebook, l.facebook_account),
                TwitterId = string.IsNullOrWhiteSpace(l.twitter_account)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.Twitter, l.twitter_account),
                YouTubeId = string.IsNullOrWhiteSpace(l.youtube_account)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.YouTube, l.youtube_account),
                Website = string.IsNullOrWhiteSpace(l.url)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.WebSite, l.url),
                ContactSite = string.IsNullOrWhiteSpace(l.contact_form)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.WebSiteContact, l.contact_form),
                BioguideId = l.id ?? string.Empty,


                //TODO RM: These do not exist in new ProPublica source
                Gender = Gender.NA,
                TermStartDate = DateTime.MinValue,
                TermEndDate = DateTime.MinValue,
                Email = new ContactMethod(ContactType.NotSet, string.Empty),


                //TODO Verify adn add checking for these values
                TotalVotes = l.total_votes ?? 0,
                MissedVotesPercent = l.missed_votes_pct ?? 0,
                VotesWithPartyPercent = l.votes_with_party_pct ?? 0,
                Senority = l.seniority ?? string.Empty
            };

            return legislator;
        }
    }
}
