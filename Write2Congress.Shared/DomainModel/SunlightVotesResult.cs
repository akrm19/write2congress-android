using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class SunlightVoteResult : SunlightBaseResult
    {
        public class Rootobject : BaseRootObject, ISunlightResult
        {
            public SunlightVote[] results { get; set; }
        }

        public class SunlightVote
        {
            /// <summary>
            /// If a vote is related to a bill, some basic fields about the bill.
            /// </summary>
            public SunlightBillResult.SunlightBill bill { get; set; }

            /// <summary>
            /// If a vote is related to a bill, the bill’s ID.
            /// </summary>
            public string bill_id { get; set; }

            public Breakdown breakdown { get; set; }

            /// <summary>
            /// The chamber the vote was taken in. “house” or “senate”.
            /// </summary>
            public string chamber { get; set; }

            /// <summary>
            /// The Congress this vote was taken in.
            /// </summary>
            public int? congress { get; set; }

            /// <summary>
            /// These fields can only occur on Senate votes, as the 
            /// Senate is the only chamber which considers presidential nominations.
            /// If a vote is related to a nomination, some basic fields about the nomination.
            /// </summary>
            public SunlighNomination nomination { get; set; }

            /// <summary>
            /// If a vote is related to a nomination, the nomination’s ID.
            /// </summary>
            public string nomination_id { get; set; }

            /// <summary>
            /// The number that vote was assigned. Numbers reset every legislative year.
            /// </summary>
            public int? number { get; set; }

            /// <summary>
            /// The official full question that the vote is addressing.
            /// </summary>
            public string question { get; set; }

            /// <summary>
            /// The required ratio of Aye votes necessary to pass the 
            /// legislation. A value of “1/2” actually means more than 
            /// 1/2. Ties are not possible in the Senate (the Vice 
            /// President casts a tie-breaker vote), and in the House, 
            /// a tie vote means the vote does not pass.
            /// </summary>
            public string required { get; set; }

            /// <summary>
            /// The official result of the vote. This is not completely 
            /// standardized (both “Passed” and “Bill Passed” may appear). 
            /// In the case of a vote for Speaker of the House, the result 
            /// field contains the name of the victor.
            /// </summary>
            public string result { get; set; }

            /// <summary>
            /// A unique identifier for a roll call vote. Made from the first letter of the chamber, the vote number, and the legislative year.
            /// </summary>
            public string roll_id { get; set; }

            /// <summary>
            /// The official description of the type of vote being taken.
            /// </summary>
            public string roll_type { get; set; }

            /// <summary>
            /// The original, official source XML for this vote information.
            /// </summary>
            public string source { get; set; }

            /// <summary>
            /// The type of vote being taken. This classification 
            /// is imperfect and unofficial, and may change as we 
            /// improve our detection. Valid types are “passage”, 
            /// “cloture”, “nomination”, “impeachment”, “treaty”, 
            /// “recommit”, “quorum”, “leadership”, and “other”.
            /// </summary>
            public string vote_type { get; set; }

            /// <summary>
            /// The time the vote was taken.
            /// </summary>
            public string voted_at { get; set; }

            /// <summary>
            /// This contains a list with the Legislator
            /// bioguideId and their vote results
            /// </summary>
            public Dictionary<string, string> voter_ids { get; set; }

            /// <summary>
            /// The “legislative year” of the vote. This is not 
            /// quite the same as the calendar year - the legislative 
            /// year changes at noon EST on January 3rd. A vote taken 
            /// on January 1, 2013 has a “legislative year” of 2012.
            /// </summary>
            public int? year { get; set; }
        }

        /// <summary>
        /// The vote breakdown gives top-level numbers about what votes were cast.
        /// Most votes are “Yea”, “Nay”, “Present”, and “Not Voting”. There are 
        /// exceptions: in the Senate, impeachment votes are “Guilty” or “Not Guilty”. 
        /// In the House, votes for the Speaker of the House are the name of the person 
        /// being voted for (e.g. “Pelosi” or “Boehner”). There may be other exceptions.
        /// Values for “Present” and “Not Voting” will always be present, no matter what 
        /// kind of vote it is. These fields are dynamic, but can all be filtered on.
        /// </summary>
        public class Breakdown
        {
            /// <summary>
            /// The number of members who cast [vote], where [vote] is a valid vote as defined above.
            /// </summary>
            public VoteBreakdown total { get; set; }

            /// <summary>
            /// * breakdown.party.[party].[vote]
            /// The number of members of[party] who cast[vote], where[party] is one of “D”, 
            /// “R”, or “I”, and[vote] is a valid vote as defined above.
            /// </summary>
            public PartyBreakdown party { get; set; }
        }

        /// <summary>
        /// * breakdown.party.[party].[vote]
        /// The number of members of[party] who cast[vote], where[party] is one of “D”, 
        /// “R”, or “I”, and[vote] is a valid vote as defined above.
        /// </summary>
        public class PartyBreakdown
        {
            public VoteBreakdown R { get; set; }
            public VoteBreakdown D { get; set; }
            public VoteBreakdown I { get; set; }
        }

        public class VoteBreakdown
        {
            public int? Yea { get; set; }
            public int? NotVoting { get; set; }
            public int? Nay { get; set; }
            public int? Present { get; set; }
        }

        public class SunlighNomination
        {
            /// <summary>
            /// An array of IDs of committees that the nomination has 
            /// been referred to for consideration.
            /// </summary>
            public string[] committee_ids { get; set; }

            /// <summary>
            /// The Congress in which this nomination was presented.
            /// </summary>
            public int? congress { get; set; }

            /// <summary>
            /// A convenience field containing only the most recent action object.
            /// </summary>
            public Last_Action last_action { get; set; }

            /// <summary>
            /// The date this nomination last received action. If there 
            /// are no official actions, then this field will fall back 
            /// to the value of received_on.
            /// </summary>
            public string last_action_at { get; set; }

            /// <summary>
            /// The unique identifier for this nomination, taken from 
            /// the Library of Congress. Of the form “PN[number]-[congress]”.
            /// </summary>
            public string nomination_id { get; set; }

            /// <summary>
            /// An array of objects with fields (described below) 
            /// about each nominee. Nominations for civil posts 
            /// tend to have only one nominee. Nominations for 
            /// military posts tend to have batches of multiple 
            /// nominees. In either case, the nominees field will 
            /// always be an array.
            /// </summary>
            public SunlightNominee[] nominees { get; set; }

            /// <summary>
            /// The number of this nomination, taken from the Library 
            /// of Congress. Can occasionally contain hyphens, e.g. “PN64-01”.
            /// </summary>
            public string number { get; set; }

            /// <summary>
            /// The organization the nominee would be appointed to, if confirmed.
            /// </summary>
            public string organization { get; set; }

            /// <summary>
            /// The date this nomination was received in the Senate.
            /// </summary>
            public string received_on { get; set; }
        }

        public class Last_Action
        {
            public string acted_at { get; set; }
            public string location { get; set; }
            public string text { get; set; }
            public string type { get; set; }
        }

        public class SunlightNominee
        {
            /// <summary>
            /// The name of the nominee, as it appears in THOMAS. 
            /// Capitalization is not consistent.
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// The position the nominee is being nominated for.
            /// </summary>
            public string position { get; set; }

            /// <summary>
            /// The which state in the United States this nominee hails 
            /// from. This field is only available for some nominees, 
            /// and never for batches of multiple nominees.
            /// </summary>
            public string state { get; set; }
        }
    }
}
