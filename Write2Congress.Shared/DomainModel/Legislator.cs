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
        public DateTime Birthday { get; set; }

        public Party Party;
        public LegislativeBody Chamber;
        public State State;
        public Gender Gender;

        public string OfficeAddress { get; set; }
        public string OfficeNumber { get; set; }

        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }

        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string YouTubeId { get; set; }

        public string Website { get; set; }
        public string ContactSite { get; set; }

        public int TotalVotes { get; set; }
        public int MissedVotesPercent { get; set; }
        public int VotesWithPartyPercent { get; set; }

        public string Senority { get; set; }
    }
}
