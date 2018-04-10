using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class Vote
    {
        #region New properties from ProPublica
        public string Description { get; set; }

        public DomainModel.VoteResults VoteResults { get; set; }
        #endregion

        public VoteCastedType VoteCastedByLegislator { get; set; }

        /// <summary>
        /// The Bill, if the vote was for a bill.
        /// </summary>
        public Bill Bill { get; set; }

        /// <summary>
        /// If a vote is related to a bill, the bill’s ID.
        /// </summary>
        public string BillId { get; set; }

        /// <summary>
        /// The chamber the vote was taken in. “house” or “senate”.
        /// </summary>
        public LegislativeBody Chamber { get; set; }

        /// <summary>
        /// NominationId if vote is for a nomination
        /// </summary>
        public string NominationId { get; set; }

        /// <summary>
        /// The nomination, if the vote was for a Nomination
        /// </summary>
        public Nomination Nomination { get; set; }

        /// <summary>
        /// The Congress this vote was taken in.
        /// </summary>
        //public string Congress { get; set; }

        /// <summary>
        /// The number that vote was assigned. Numbers reset every legislative year.
        /// </summary>
        //public int Number { get; set; }

        /// <summary>
        /// The official full question that the vote is addressing.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// The required ratio of Aye votes necessary to pass the 
        /// legislation. A value of “1/2” actually means more than 1/2. 
        /// Ties are not possible in the Senate (the Vice President 
        /// casts a tie-breaker vote), and in the House, a tie vote 
        /// means the vote does not pass.
        /// </summary>
        //public string Required { get; set; }

        /// <summary>
        /// The official result of the vote. This is not completely 
        /// standardized (both “Passed” and “Bill Passed” may appear). 
        /// In the case of a vote for Speaker of the House, the result 
        /// field contains the name of the victor.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// A unique identifier for a roll call vote. Made from the first 
        /// letter of the chamber, the vote number, and the legislative year.
        /// </summary>
        //public string RollId { get; set; }

        /// <summary>
        /// The official description of the type of vote being taken.
        /// </summary>
        //public string RollType { get; set; }

        /// <summary>
        /// The original, official source XML for this vote information.
        /// </summary>
        public string Source { get; set; }


        /// <summary>
        /// The type of vote being taken. This classification is imperfect 
        /// and unofficial, and may change as we improve our detection. 
        /// Valid types are “passage”, “cloture”, “nomination”, “impeachment”, 
        /// “treaty”, “recommit”, “quorum”, “leadership”, and “other”.
        /// </summary>
        //public VoteType Type { get; set; }

        /// <summary>
        /// The time the vote was taken.
        /// </summary>
        public DateTime VotedAt { get; set; }

        /// <summary>
        /// The “legislative year” of the vote. This is not quite the 
        /// same as the calendar year - the legislative year changes 
        /// at noon EST on January 3rd. A vote taken on January 1, 2013 
        /// has a “legislative year” of 2012.
        /// </summary>
        //public int Year { get; set; }


        public Vote() { }

        public static Vote TransformToVote(IVote vote)
        {
            var newVote = new Vote
            {
                Bill = Bill.TransformToBill(vote.Bill),
                BillId = vote.BillId,
                Chamber = vote.Chamber,
                Description = vote.Description,
                Nomination = vote.Nomination,
                NominationId = vote.NominationId,
                Question = vote.Question,
                Result = vote.Result,
                Source = vote.Source,
                //Type = vote.Type,
                VoteCastedByLegislator = vote.VoteCastedByLegislator,
                VotedAt = vote.VotedAt,
                VoteResults = vote.VoteResult
            };

            return newVote;
        }
    }
}
