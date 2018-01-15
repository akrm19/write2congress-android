using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public abstract class BaseLegislatorsResult : BaseResult
    {
        public abstract class BaseResult
        {
            public string congress { get; set; }
            public string chamber { get; set; }
            public int? num_results { get; set; }
            public int? offset { get; set; }
            //public virtual Member[] members { get; set; }
        }
        
        public abstract class Member : ILegislator
        {
            //public bool at_large { get; set; }
            public bool in_office { get; set; }
            public float? dw_nominate { get; set; }
            public float? missed_votes_pct { get; set; }
            public float? votes_with_party_pct { get; set; }
            public int? missed_votes { get; set; }
            public int? total_present { get; set; }
            public int? total_votes { get; set; }
            public object ideal_point { get; set; }
            public string api_uri { get; set; }
            public string contact_form { get; set; }
            public string crp_id { get; set; }
            public string cspan_id { get; set; }
            public string date_of_birth { get; set; }
            //public string district { get; set; }
            public string facebook_account { get; set; }
            public string fax { get; set; }
            public string fec_candidate_id { get; set; }
            public string first_name { get; set; }
            //public string geoid { get; set; }
            public string google_entity_id { get; set; }
            public string govtrack_id { get; set; }
            public string icpsr_id { get; set; }
            public string id { get; set; }
            public string last_name { get; set; }
            public string leadership_role { get; set; }
            //public string lis_id { get; set; }
            public string middle_name { get; set; }
            public string next_election { get; set; }
            public string ocd_id { get; set; }
            public string office { get; set; }
            public string party { get; set; }
            public string phone { get; set; }
            public string rss_url { get; set; }
            //public string senate_class { get; set; }
            public string seniority { get; set; }
            public string short_title { get; set; }
            public string state { get; set; }
            //public string state_rank { get; set; }
            public string suffix { get; set; }
            public string title { get; set; }
            public string twitter_account { get; set; }
            public string url { get; set; }
            public string votesmart_id { get; set; }
            public string youtube_account { get; set; }


            string ILegislator.FirstName
            {
                get { return first_name ?? string.Empty; }  
                set { first_name = value; }
            }
            string ILegislator.MiddleName
            {
                get { return middle_name ?? string.Empty; }
                set { middle_name = value; }
            }
            string ILegislator.LastName
            {
                get { return last_name ?? string.Empty; }
                set { last_name = value; }
            }
            DateTime ILegislator.Birthday
            {
                get { return DataTransformationUtil.DateFromSunlightTime(date_of_birth); }
                set {}
            }
            Party ILegislator.Party
            {
                get { return DataTransformationUtil.PartyFromString(party); }
                set {}
            }

            LegislativeBody ILegislator.Chamber { get; set; }

            StateOrTerritory ILegislator.State
            {
                get { return DataTransformationUtil.GetStateOrTerritoryFromSunlight(state); }
                set { }
            }
            Gender ILegislator.Gender
            {
                get { return Gender.NA; }
                set { }
            }
            DateTime ILegislator.TermStartDate
            {
                get { return DateTime.MinValue; }
                set { }
            }
            DateTime ILegislator.TermEndDate
            {
                get { return DateTime.MinValue; }
                set { }
            }
            ContactMethod ILegislator.OfficeAddress
            {
                get { return string.IsNullOrWhiteSpace(office)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Mail, office); }
                set { }
            }
            ContactMethod ILegislator.OfficeNumber
            {
                get { return string.IsNullOrWhiteSpace(phone)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Phone, phone); }
                set { }
            }
            ContactMethod ILegislator.Email
            {
                get { return new ContactMethod(ContactType.NotSet, string.Empty); }
                set { }
            }
            ContactMethod ILegislator.FacebookId
            {
                get { return string.IsNullOrWhiteSpace(facebook_account)
                        ? new ContactMethod(ContactType.NotSet, string.Empty)
                        : new ContactMethod(ContactType.Facebook, facebook_account); }
                set { }
            }
            ContactMethod ILegislator.TwitterId
            {
                get { return string.IsNullOrWhiteSpace(twitter_account)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.Twitter, twitter_account); }
                set { }
            }
            ContactMethod ILegislator.YouTubeId
            {
                get { return string.IsNullOrWhiteSpace(youtube_account)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.YouTube, youtube_account); }
                set { }
            }
            ContactMethod ILegislator.Website
            {
                get { return string.IsNullOrWhiteSpace(url)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.WebSite, url); }
                set { }
            }
            ContactMethod ILegislator.ContactSite
            {
                get { return string.IsNullOrWhiteSpace(contact_form)
                     ? new ContactMethod(ContactType.NotSet, string.Empty)
                     : new ContactMethod(ContactType.WebSiteContact, contact_form); }
                set { }
            }
            int ILegislator.TotalVotes
            {
                get { return total_votes ?? 0; }
                set { total_votes = value; }
            }
            float ILegislator.MissedVotesPercent
            {
                get { return missed_votes_pct ?? 0; }
                set { missed_votes_pct = value; }
            }
            float ILegislator.VotesWithPartyPercent
            {
                get { return votes_with_party_pct ?? 0; }
                set { votes_with_party_pct = value; }
            }
            string ILegislator.Senority
            {
                get { return seniority ?? string.Empty; }
                set { seniority = value; }
            }
            string ILegislator.IdBioguide
            {
                get { return id ?? string.Empty; }
                set { id = value; }
            }
            string ILegislator.IdGovTrack
            {
                get { return govtrack_id ?? string.Empty; }
                set { govtrack_id = value; }
            }
            string ILegislator.IdThomas
            {
                get;  set;
            }
            string ILegislator.IdVoteSmart
            {
                get; set;
            }
            string ILegislator.IdOpenSecrets
            {
                get; set;
            }
        }
    }
}
