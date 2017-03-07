using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public class Util
    {
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

        public static Party PartyFromString(string party)
        {
            switch (party.ToLower())
            {
                case "r":
                case "republican":
                    return Party.Republican;
                case "d":
                case "democrat":
                    return Party.Democratic;
                case "l":
                case "libertarian":
                    return Party.Libertarian;
                case "g":
                case "green":
                    return Party.Green;
                default:
                    return Party.Unknown;
            }
        }

        public static Gender GenderFromString(string gender)
        {
            switch (gender.ToLower())
            {
                case "m":
                case "male":
                    return Gender.Male;
                case "f":
                case "female":
                    return Gender.Female;
                default:
                    return Gender.NA;
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
        public static List<Legislator> LegislatorsFromSunlightLegislatorResult(SunlightLegislatorResult legislatorResults)
        {
            var legislators = new List<Legislator>();


            foreach (var l in legislatorResults.results)
            {
                var legislator = new Legislator()
                {
                    FirstName = l.first_name,
                    MiddleName = l.middle_name,
                    LastName = l.last_name,
                    Birthday = Util.DateFromSunlightTime(l.birthday),
                    Party = Util.PartyFromString(l.party),
                    Chamber = Util.GetLegislativeBodyFromSunlight(l.chamber),
                    State = Util.GetStateOrTerritoryFromSunlight(l.state),
                    Gender = Util.GenderFromString(l.gender),
                    TermStartDate = Util.DateFromSunlightTime(l.term_start),
                    TermEndDate = Util.DateFromSunlightTime(l.term_end),
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


                    TotalVotes = 0, //TODO: get rid of the following or populate
                    MissedVotesPercent = 0,
                    VotesWithPartyPercent = 0,
                    Senority = string.Empty
                };

                legislators.Add(legislator);
            }

            return legislators;
        }

        public static DateTime DateFromSunlightTime(string dateVal)
        {
            DateTime date;

            return DateTime.TryParseExact(dateVal, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date)
                ? date
                : DateTime.MinValue;
        }

        public static LegislativeBody GetLegislativeBodyFromSunlight(string chamber)
        {
            switch (chamber.ToLower())
            {
                case "senate":
                    return LegislativeBody.Senate;
                case "house":
                    return LegislativeBody.House;
                default:
                    return LegislativeBody.Unknown;
            }
        }

        public static StateOrTerritory GetStateOrTerritoryFromSunlight(string stateOrTerritory)
        {
            StateOrTerritory result = StateOrTerritory.ALL;
            //TODO RM: handle unkonw state or territory
            if (string.IsNullOrWhiteSpace(stateOrTerritory))
                return result;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerritory, true, out result))
                return result;

            //TODO RM: Handle errors
            return result;
        }
        #endregion
    }
}
