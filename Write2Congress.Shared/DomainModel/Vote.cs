using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class Vote
    {
        /// <summary>
        /// The chamber the vote was taken in. “house” or “senate”.
        /// </summary>
        public LegislativeBody chamber { get; set; }

        /// <summary>
        /// The Congress this vote was taken in.
        /// </summary>
        public string congress { get; set; }

        /// <summary>
        /// The number that vote was assigned. Numbers reset every legislative year.
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// The official full question that the vote is addressing.
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// The required ratio of Aye votes necessary to pass the 
        /// legislation. A value of “1/2” actually means more than 1/2. 
        /// Ties are not possible in the Senate (the Vice President 
        /// casts a tie-breaker vote), and in the House, a tie vote 
        /// means the vote does not pass.
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
        /// A unique identifier for a roll call vote. Made from the first 
        /// letter of the chamber, the vote number, and the legislative year.
        /// </summary>
        public string roll_id { get; set; }


        public string roll_type { get; set; }

        /// <summary>
        /// The original, official source XML for this vote information.
        /// </summary>
        public string source { get; set; }
        public string url { get; set; }
        public string vote_type { get; set; }
        public DateTime voted_at { get; set; }
        public int year { get; set; }

        /// <summary>
        /// If a vote is related to a bill, the bill’s ID.
        /// </summary>
        public string bill_id { get; set; }
    }
}
