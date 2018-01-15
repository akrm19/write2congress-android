using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel.ApiModels.UnitedStatesIo
{
    public class CongressLegislatorsResult
    {
        public class Rootobject: ILegislatorResult
        {
            public Results[] results { get; set; }

            List<ILegislator> ILegislatorResult.GetLegislatorsResult()
            {
                var legislators = new List<ILegislator>();

                legislators.AddRange(results);

                return legislators;
            }
        }

        public class Results : ILegislator
        {
            public Id id { get; set; }
            public Name name { get; set; }
            public Bio bio { get; set; }
            public Term[] terms { get; set; }
            public Other_Names[] other_names { get; set; }
            public Leadership_Roles[] leadership_roles { get; set; }
            public Family[] family { get; set; }

            string ILegislator.FirstName
            {
                get
                {
                    return name.first ?? string.Empty;
                }
                set
                {
                    name.first = value;
                }
            }
            string ILegislator.MiddleName
            {
                get
                {
                    return name.middle ?? string.Empty;
                }
                set
                {
                    name.middle = value;
                }
            }
            string ILegislator.LastName
            {
                get
                {
                    return name.last ?? string.Empty;
                }
                set
                {
                    name.last = value;
                }
            }
            DateTime ILegislator.Birthday
            {
                get
                {
                    return DataTransformationUtil.DateFromSunlightTime(bio.birthday);
                }
                set { }
            }
            Party ILegislator.Party
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    return latestTerm == null
                        ? Party.Unknown
                        : DataTransformationUtil.PartyFromString(latestTerm.party);
                }
                set { }
            }
            LegislativeBody ILegislator.Chamber
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    return latestTerm == null
                        ? LegislativeBody.Unknown
                        : DataTransformationUtil.LegislativeBodyFromSunlight(latestTerm.type);
                }
                set { }
            }
            StateOrTerritory ILegislator.State
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    return latestTerm == null
                        ? StateOrTerritory.ALL
                        : DataTransformationUtil.GetStateOrTerritoryFromSunlight(latestTerm.state);
                }
                set { }
            }
            Gender ILegislator.Gender
            {
                get
                {
                    return DataTransformationUtil.GenderFromString(bio?.gender ?? string.Empty);
                }
                set { }
            }
            DateTime ILegislator.TermStartDate
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    return latestTerm == null
                        ? DateTime.MinValue
                        : DataTransformationUtil.DateFromSunlightTime(latestTerm?.start ?? string.Empty);
                }
                set { }
            }
            DateTime ILegislator.TermEndDate
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    return latestTerm == null
                        ? DateTime.MinValue
                        : DataTransformationUtil.DateFromSunlightTime(latestTerm?.end ?? string.Empty);
                }
                set { }
            }
            ContactMethod ILegislator.OfficeAddress
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    if (latestTerm == null || string.IsNullOrWhiteSpace(latestTerm.address))
                        return new ContactMethod(ContactType.NotSet, string.Empty);

                    return new ContactMethod(ContactType.Mail, latestTerm.address);
                }
                set { }
            }
            ContactMethod ILegislator.OfficeNumber
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    if (latestTerm == null || string.IsNullOrWhiteSpace(latestTerm.phone))
                        return new ContactMethod(ContactType.NotSet, string.Empty);

                    return new ContactMethod(ContactType.Phone, latestTerm.phone);
                }
                set { }
            }
            ContactMethod ILegislator.Email
            {
                get
                {
                    return new ContactMethod(ContactType.NotSet, string.Empty);
                }
                set { }
            }
            ContactMethod ILegislator.FacebookId
            {
                get
                {
                    return new ContactMethod(ContactType.NotSet, string.Empty);
                }
                set { }
            }
            ContactMethod ILegislator.TwitterId
            {
                get
                {
                    return new ContactMethod(ContactType.NotSet, string.Empty);
                }
                set { }
            }
            ContactMethod ILegislator.YouTubeId
            {
                get
                {
                    return new ContactMethod(ContactType.NotSet, string.Empty);
                }
                set { }
            }
            ContactMethod ILegislator.Website
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    if (latestTerm == null || string.IsNullOrWhiteSpace(latestTerm.url))
                        return new ContactMethod(ContactType.NotSet, string.Empty);

                    return new ContactMethod(ContactType.Phone, latestTerm.url);
                }
                set { }
            }
            ContactMethod ILegislator.ContactSite
            {
                get
                {
                    var latestTerm = GetLastestTerm();

                    if (latestTerm == null || string.IsNullOrWhiteSpace(latestTerm.contact_form))
                        return new ContactMethod(ContactType.NotSet, string.Empty);

                    return new ContactMethod(ContactType.WebSiteContact, latestTerm.contact_form);
                }
                set { }
            }
            int ILegislator.TotalVotes                  { get; set; }
            float ILegislator.MissedVotesPercent        { get; set; }
            float ILegislator.VotesWithPartyPercent     { get; set; }
            string ILegislator.Senority { get; set; } = string.Empty;
            string ILegislator.IdBioguide
            {
                get => id.bioguide ?? string.Empty;
                set => id.bioguide = value;
            }
            string ILegislator.IdGovTrack
            {
                get => id.govtrack.ToString() ?? string.Empty;
                set => id.govtrack = int.Parse(value);
            }
            string ILegislator.IdThomas
            {
                get => id.thomas ?? string.Empty;
                set => id.thomas = value;
            }
            string ILegislator.IdVoteSmart
            {
                get
                {
                    return id.votesmart.ToString() ?? string.Empty;
                }
                set => id.votesmart = int.Parse(value);
            }
            string ILegislator.IdOpenSecrets
            {
                get
                {
                    return id.opensecrets ?? string.Empty;
                }
                set => id.opensecrets = value;
            }

            private Term GetLastestTerm()
            {
                if (terms == null || terms.Count() == 0)
                    return null;

                var lastest =
                    from term in terms
                    where DataTransformationUtil.DateFromSunlightTime(term.end) >= DateTime.Now
                    orderby DataTransformationUtil.DateFromSunlightTime(term.end) descending
                    select term;

                return lastest.FirstOrDefault();
            }
        }

        public class Id
        {
            public int? cspan { get; set; }
            public int? govtrack { get; set; }
            public int? icpsr { get; set; }
            public int? maplight { get; set; }
            public int? votesmart { get; set; }
            public long? house_history { get; set; }
            public string ballotpedia { get; set; }
            public string bioguide { get; set; }
            public string google_entity_id { get; set; }
            public string lis { get; set; }
            public string opensecrets { get; set; }
            public string thomas { get; set; }
            public string wikidata { get; set; }
            public string wikipedia { get; set; }
            public string[] fec { get; set; }
        }

        public class Name
        {
            public string first { get; set; }
            public string last { get; set; }
            public string official_full { get; set; }
            public string middle { get; set; }
            public string nickname { get; set; }
            public string suffix { get; set; }
        }

        public class Bio
        {
            public string birthday { get; set; }
            public string gender { get; set; }
            public string religion { get; set; }
        }

        public class Term
        {
            public int? _class { get; set; }
            public int? district { get; set; }
            public Party_Affiliations[] party_affiliations { get; set; }
            public string address { get; set; }
            public string caucus { get; set; }
            public string contact_form { get; set; }
            public string end { get; set; }
            public string fax { get; set; }
            public string office { get; set; }
            public string party { get; set; }
            public string phone { get; set; }
            public string rss_url { get; set; }
            public string start { get; set; }
            public string state { get; set; }
            public string state_rank { get; set; }
            public string type { get; set; }
            public string url { get; set; }
        }

        public class Party_Affiliations
        {
            public string start { get; set; }
            public string end { get; set; }
            public string party { get; set; }
        }

        public class Other_Names
        {
            public string last { get; set; }
        }

        public class Leadership_Roles
        {
            public string title { get; set; }
            public string chamber { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

        public class Family
        {
            public string name { get; set; }
            public string relation { get; set; }
        }
    }
}
