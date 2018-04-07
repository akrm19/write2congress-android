using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Nominee
    {
        public Nominee()
        {
        }

        public Nominee(string name, string position, string state = "") 
        {
            Name = name;
            Position = position;
            State = string.IsNullOrWhiteSpace(state)
                ? string.Empty
                : state;
        }

        /// <summary>
        /// The name of the nominee, as it appears in THOMAS. 
        /// Capitalization is not consistent.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The position the nominee is being nominated for.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// The which state in the United States this nominee hails 
        /// from. This field is only available for some nominees, 
        /// and never for batches of multiple nominees.
        /// </summary>
        public string State { get; set; }
    }
}
