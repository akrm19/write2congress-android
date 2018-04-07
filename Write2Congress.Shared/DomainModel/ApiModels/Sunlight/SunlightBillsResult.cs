using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class SunlightBillResult : SunlightBaseResult
    {   
        public class Rootobject : BaseRootObject, ISunlightResult
        {
            public SunlightBill[] results { get; set; }
        }

        public class SunlightBill
        {
            /// <summary>
            /// The actions field has a list of all official activity that has occurred to a bill. 
            /// All fields are parsed out of non-standardized sentence text, so mistakes and omissions are possible.
            /// </summary>
            public Action[] actions { get; set; }

            /// <summary>
            /// The unique ID for this bill. Formed from the bill_type, number, and congress.
            /// </summary>
            public string bill_id { get; set; }
            
            /// <summary>
            /// The type for this bill. For the bill “H.R. 4921”, the bill_type represents the 
            /// “H.R.” part. Bill types can be: hr, hres, hjres, hconres, s, sres, sjres, sconres.
            /// </summary>
            public string bill_type { get; set; }
            
            /// <summary>
            /// The chamber in which the bill originated.
            /// </summary>
            public string chamber { get; set; }

            /// <summary>
            /// A list of IDs of committees related to this bill.
            /// </summary>
            public string[] committee_ids { get; set; }
            
            /// <summary>
            /// The Congress in which this bill was introduced. For example, bills introduced in the “111th Congress” have a congress of 111.
            /// </summary>
            public int? congress { get; set; }

            /// <summary>
            /// An array of bioguide IDs for each cosponsor of the bill. 
            /// Bills do not always have cosponsors.
            /// </summary>
            public string[] cosponsor_ids { get; set; }

            /// <summary>
            /// The number of active cosponsors of the bill.
            /// </summary>
            public int? cosponsors_count { get; set; }

            /// <summary>
            /// If a bill has been enacted into law, the enacted_as field contains 
            /// information about the law number it was assigned. The above information 
            /// is for Public Law 111-148.
            /// </summary>
            public Enacted_As enacted_as { get; set; }

            /// <summary>
            /// The history field includes useful flags and dates/times 
            /// in a bill’s life. The above is a real-life example 
            /// of H.R. 3590 - not all fields will be present for every bill.
            /// Time fields can hold either dates or times - Congress is 
            /// inconsistent about providing specific timestamps
            /// </summary>
            public History history { get; set; }

            /// <summary>
            /// The date this bill was introduced.
            /// </summary>
            public string introduced_on { get; set; }

            /// <summary>
            /// A list of official keywords and phrases assigned by the 
            /// Library of Congress. These keywords can be used to group 
            /// bills into tags or topics, but there are many of them 
            /// (1,023 unique keywords since 2009, as of late 2012), and 
            /// they are not grouped into a hierarchy. They can be assigned 
            /// or revised at any time after introduction.
            /// </summary>
            public string[] keywords { get; set; }

            /// <summary>
            /// The most recent action.
            /// </summary>
            public Action last_action { get; set; }

            /// <summary>
            /// The date or time of the most recent vote on this bill.
            /// </summary>
            public string last_action_at { get; set; }

            /// <summary>
            /// The date or time of the most recent official action. In the rare case 
            /// that there are no official actions, this field will be set to the value 
            /// of introduced_on.
            /// </summary>
            public string last_vote_at { get; set; }

            /// <summary>
            /// Information for only the most recent version of a bill. 
            /// Useful to limit the size of a request with partial responses.
            /// </summary>
            public Last_Version last_version { get; set; }

            /// <summary>
            /// The date the last version of this bill was published. This will be set 
            /// to the introduced_on date until an official version of the bill’s text is published.
            /// </summary>
            public string last_version_on { get; set; }
            
            /// <summary>
            /// The number for this bill. For the bill “H.R. 4921”, the number is 4921.
            /// </summary>
            public int? number { get; set; }

            /// <summary>
            /// The current official title of a bill. Official titles are sentences. 
            /// Always present. Assigned at introduction, and can be revised any time.
            /// </summary>
            public string official_title { get; set; }

            /// <summary>
            /// The current popular handle of a bill, as denoted by the Library of Congress. 
            /// They are rare, and are assigned by the LOC for particularly ubiquitous bills. 
            /// They are non-capitalized descriptive phrases. They can be assigned any time.
            /// </summary>
            public string popular_title { get; set; }

            /// <summary>
            /// An array of common nicknames for a bill that don’t appear in official data. 
            /// These nicknames are sourced from a public dataset at unitedstates/bill-nicknames, 
            /// and will only appear for a tiny fraction of bills. In the future, we plan to 
            /// auto-generate acronyms from bill titles and add them to this array.
            /// </summary>
            public string[] Nicknames { get; set; }

            /// <summary>
            /// A list of IDs of bills that the Library of Congress 
            /// has declared “related”. Relations can be pretty loose, 
            /// use this field with caution.
            /// </summary>
            public string[] related_bill_ids { get; set; }

            /// <summary>
            /// The current shorter, catchier title of a bill. About half of bills get these, 
            /// and they can be assigned any time.
            /// </summary>
            public string short_title { get; set; }


            //TODO RM: This is a Legislator Result, update it so it uses that instead
            /// <summary>
            /// An object with most simple legislator fields for the bill’s sponsor, if there is one.
            /// </summary>
            public Legislator sponsor { get; set; }

            /// <summary>
            /// The bioguide ID of the bill’s sponsoring legislator, 
            /// if there is one. It is possible, but rare, to have bills with no sponsor.
            /// </summary>
            public string sponsor_id { get; set; }

            /// <summary>
            /// An official summary written and assigned at some point after introduction by 
            /// the Library of Congress. These summaries are generally more accessible than 
            /// the text of the bill, but can still be technical. The LOC does not write 
            /// summaries for all bills, and when they do can assign and revise them at any time.
            /// </summary>
            public string summary { get; set; }

            /// <summary>
            /// The official summary, but capped to 1,000 characters (and an ellipse). 
            /// Useful when you want to show only the first part of a bill’s summary, 
            /// but don’t want to download a potentially large amount of text.
            /// </summary>
            public string summary_short { get; set; }

            public Title[] titles { get; set; }

            /// <summary>
            /// An object with URLs for this bill’s landing page on Congress.gov, GovTrack.us, and OpenCongress.org.
            /// </summary>
            public Urls urls { get; set; }

            public Vote[] votes { get; set; }

            /// <summary>
            /// An array of bioguide IDs for each legislator who has 
            /// withdrawn their cosponsorship of the bill.
            /// </summary>
            public string[] withdrawn_cosponsor_ids { get; set; }

            /// <summary>
            /// The number of withdrawn cosponsors of the bill.
            /// </summary>
            public int? withdrawn_cosponsors_count { get; set; }

            /// <summary>
            /// The upcoming field has an array of objects describing 
            /// when a bill has been scheduled for future debate on 
            /// the House or Senate floor. Its information is taken from
            /// party leadership websites in the House and Senate, and 
            /// updated frequently throughout the day.
            /// While this information is official, party leadership in 
            /// both chambers have unilateral and immediate control over 
            /// what is scheduled on the floor, and it can change at any time.
            /// We do our best to automatically remove entries when a bill has 
            /// been yanked from the floor schedule.
            /// </summary>
            public Upcoming[] upcoming { get; set; }
        }

        public class Action
        {
            /// <summary>
            /// The date or time the action occurred. Always present.
            /// </summary>
            public string acted_at { get; set; }

            public string action_code { get; set; }

            public string calendar { get; set; }
            
            /// <summary>
            /// If the action is a vote, which chamber this vote occured in. “house” or “senate”.
            /// </summary>
            public string chamber { get; set; }

            public string congress { get; set; }

            /// <summary>
            /// A list of subobjects containing committee_id and name fields 
            /// for any committees referenced in an action. Will be missing 
            /// if no committees are mentioned.
            /// </summary>
            public SunlightCommitteeResult.SunlightCommittee[] committees { get; set; }

            /// <summary>
            /// If the action is a vote, how the vote was taken. Can be “roll”, “voice”, or “Unanimous Consent”.
            /// </summary>
            public string how { get; set; }

            public string law { get; set; }

            public string number { get; set; }

            /// <summary>
            /// A list of references to the Congressional Record that this action links to.
            /// </summary>
            public Reference[] references { get; set; }

            /// <summary>
            /// If the action is a vote, the result. “pass” or “fail”.
            /// </summary>
            public string result { get; set; }

            /// <summary>
            /// The official text that describes this action. Always present.
            /// </summary>
            public string text { get; set; }

            /// <summary>
            /// The type of action. Always present. Can be “action” (generic), 
            /// “vote” (passage vote), “vote-aux” (cloture vote), “vetoed”, 
            /// “topresident”, and “enacted”. There can be other values, but 
            /// these are the only ones we support.
            /// </summary>
            public string type { get; set; }

            public string under { get; set; }

            /// <summary>
            /// If the action is a vote, this is the type of vote. “vote”, “vote2”, “cloture”, or “pingpong”.
            /// </summary>
            public string vote_type { get; set; }

            /// <summary>
            /// If the action is a roll call vote, the ID of the roll call.
            /// </summary>
            public string roll_id { get; set; }

            public bool? suspension { get; set; }
        }

        public class Urls
        {
            public string congress { get; set; }
            public string govtrack { get; set; }
        }

        public class Enacted_As
        {
            /// <summary>
            /// The Congress in which this bill was enacted into law.
            /// </summary>
            public int congress { get; set; }

            /// <summary>
            /// Whether the law is a public or private law. Most laws 
            /// are public laws; private laws affect individual citizens. “public” or “private”.
            /// </summary>
            public string law_type { get; set; }

            /// <summary>
            /// The number the law was assigned.
            /// </summary>
            public int number { get; set; }
        }

        /// <summary>
        /// The history field includes useful flags and dates/times 
        /// in a bill’s life. The above is a real-life example 
        /// of H.R. 3590 - not all fields will be present for every bill.
        /// Time fields can hold either dates or times - Congress is 
        /// inconsistent about providing specific timestamps
        /// </summary>
        public class History
        {
            /// <summary>
            /// Whether this bill has had any action beyond the standard 
            /// action all bills get (introduction, referral to committee, 
            /// sponsors’ introductory remarks). Only a small percentage of 
            /// bills get this additional activity.
            /// </summary>
            public bool active { get; set; }

            /// <summary>
            /// If this bill got any action beyond initial introduction, the 
            /// date or time of the first such action. This field will stay 
            /// constant even as further action occurs. For the time of the most 
            /// recent action, look to the last_action_at field.
            /// </summary>
            public string active_at { get; set; }

            /// <summary>
            /// Whether the bill is currently awaiting the President’s signature. Always present.
            /// </summary>
            public bool awaiting_signature { get; set; }

            /// <summary>
            /// Whether the bill has been enacted into law. Always present.
            /// </summary>
            public bool enacted { get; set; }

            /// <summary>
            /// The date or time the bill was enacted into law. Only present if this happened.
            /// </summary>
            public string enacted_at { get; set; }

            /// <summary>
            /// The result of the last time the House voted on passage. 
            /// Only present if this vote occurred. “pass” or “fail”.
            /// </summary>
            public string house_passage_result { get; set; }

            /// <summary>
            /// The date or time the House last voted on passage. Only present if this vote occurred.
            /// </summary>
            public string house_passage_result_at { get; set; }

            /// <summary>
            /// The result of the last time the Senate voted on passage. 
            /// Only present if this vote occurred. “pass” or “fail”.
            /// </summary>
            public string senate_passage_result { get; set; }

            /// <summary>
            /// The date or time the Senate last voted on passage. Only present if this vote occurred.
            /// </summary>
            public string senate_passage_result_at { get; set; }

            /// <summary>
            /// The result of the last time the Senate voted on cloture. 
            /// Only present if this vote occurred. “pass” or “fail”.
            /// </summary>
            public string senate_cloture_result { get; set; }

            /// <summary>
            /// The date or time the Senate last voted on cloture.
            /// Only present if this vote occurred.
            /// </summary>
            public string senate_cloture_result_at { get; set; }

            /// <summary>
            /// The date or time the bill began awaiting the President’s signature. 
            /// Only present if this happened.
            /// </summary>
            public string awaiting_signature_since { get; set; }

            /// <summary>
            /// The result of the last time the House voted to override a veto. 
            /// Only present if this vote occurred. “pass” or “fail”.
            /// </summary>
            public string house_override_result { get; set; }

            /// <summary>
            /// The date or time the House last voted to override a veto. Only present if this vote occurred.
            /// </summary>
            public string house_override_result_at { get; set; }

            /// <summary>
            /// The result of the last time the Senate voted to override a veto. 
            /// Only present if this vote occurred. “pass” or “fail”.
            /// </summary>
            public string senate_override_result { get; set; }

            /// <summary>
            /// The date or time the Senate last voted to override a veto. Only present if this vote occurred.
            /// </summary>
            public string senate_override_result_at { get; set; }

            /// <summary>
            /// Whether the bill has been vetoed by the President. Always present.
            /// </summary>
            public bool vetoed { get; set; }

            /// <summary>
            /// The date or time the bill was vetoed by the President. Only present if this happened.
            /// </summary>
            public string vetoed_at { get; set; }
        }

        public class Reference
        {
            public string reference { get; set; }
            public string type { get; set; }
        }

        public class Last_Version
        {
            public int pages { get; set; }
            public string bill_version_id { get; set; }
            public string issued_on { get; set; }
            public string version_code { get; set; }
            public string version_name { get; set; }
            public Urls1 urls { get; set; }
        }

        public class Urls1
        {
            public string html { get; set; }
            public string pdf { get; set; }
            public string xml { get; set; }
        }

        public class Title
        {
            public string _as { get; set; }
            public bool is_for_portion { get; set; }
            public string title { get; set; }
            public string type { get; set; }
        }

        public class Upcoming
        {
            public string scheduled_at { get; set; }

            /// <summary>
            /// What Congress this is occurring in.
            /// </summary>
            public int congress { get; set; }
            public string bill_url { get; set; }

            /// <summary>
            /// What chamber the bill is scheduled for debate in.
            /// </summary>
            public string chamber { get; set; }
            public string consideration { get; set; }

            /// <summary>
            /// Some surrounding context of why the bill is scheduled. 
            /// This is only present for Senate updates right now.
            /// </summary>
            public string context { get; set; }
            public string description { get; set; }
            public string floor_id { get; set; }

            /// <summary>
            /// The date the bill is scheduled for floor debate.
            /// </summary>
            public string legislative_day { get; set; }

            /// <summary>
            /// How precise this information is. “day”, “week”, or null.
            /// See more details on this field in the /upcoming_bills documentation.
            /// </summary>
            public string range { get; set; }

            /// <summary>
            /// Where this information is coming from. Currently, 
            /// the only values are “senate_daily” or “house_daily”.
            /// </summary>
            public string source_type { get; set; }

            /// <summary>
            /// An official reference URL for this information.
            /// </summary>
            public string url { get; set; }
        }
    }
}
