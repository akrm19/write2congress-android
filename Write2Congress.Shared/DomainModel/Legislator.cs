using System;
using System.Collections.Generic;
using System.Text;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class Legislator
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public Party Party;
        public State State;
        public Gender Gender;

        public string OfficeAddress { get; set; }
        public string OfficeNumber { get; set; }

        public string TermStartDate { get; set; }
        public string NextElection { get; set; }

        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string YouTube { get; set; }

        public int TotalVotes { get; set; }
        public int MissedVotesPercent { get; set; }
        public int VotesWithPartyPercent { get; set; }

        public string Senority { get; set; }
    }
}
