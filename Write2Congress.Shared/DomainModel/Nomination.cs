using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Nomination
    {
        /// <summary>
        /// The date this nomination was received in the Senate.
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// The date this nomination last received action. If there 
        /// are no official actions, then this field will fall back 
        /// to the value of received_on.
        /// </summary>
        public DateTime DateOfLastAction { get; set; }

        /// <summary>
        /// An List of Nominee with fields (described below) 
        /// about each nominee. Nominations for civil posts 
        /// tend to have only one nominee. Nominations for 
        /// military posts tend to have batches of multiple 
        /// nominees. In either case, the nominees field will 
        /// always be an array.
        /// </summary>
        public List<Nominee> Nominees { get; set; }

        /// <summary>
        /// The organization the nominee would be appointed to, if confirmed.
        /// </summary>
        public string Organization { get; set; }
    }
}
