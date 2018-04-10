using System;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IVote
    {
        string Description { get; set; }

        DomainModel.VoteResults VoteResult { get; }

        VoteCastedType VoteCastedByLegislator { get; }

        /// <summary>
        /// The Bill, if the vote was for a bill.
        /// </summary>
        IBill Bill { get; }

        /// <summary>
        /// If a vote is related to a bill, the bill’s ID.
        /// </summary>
        string BillId { get; set; }

        /// <summary>
        /// The chamber the vote was taken in. “house” or “senate”.
        /// </summary>
        LegislativeBody Chamber { get; set; }

        /// <summary>
        /// NominationId if vote is for a nomination
        /// </summary>
        string NominationId { get; set; }

        /// <summary>
        /// The nomination, if the vote was for a Nomination
        /// </summary>
        Nomination Nomination { get; set; }

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
        string Question { get; set; }

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
        string Result { get; set; }

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
        string Source { get; set; }


        /// <summary>
        /// The type of vote being taken. This classification is imperfect 
        /// and unofficial, and may change as we improve our detection. 
        /// Valid types are “passage”, “cloture”, “nomination”, “impeachment”, 
        /// “treaty”, “recommit”, “quorum”, “leadership”, and “other”.
        /// </summary>
        //VoteType Type { get; set; }

        /// <summary>
        /// The time the vote was taken.
        /// </summary>
        DateTime VotedAt { get; set; }

        /// <summary>
        /// The “legislative year” of the vote. This is not quite the 
        /// same as the calendar year - the legislative year changes 
        /// at noon EST on January 3rd. A vote taken on January 1, 2013 
        /// has a “legislative year” of 2012.
        /// </summary>
        //int Year { get; set; }
    }
}
