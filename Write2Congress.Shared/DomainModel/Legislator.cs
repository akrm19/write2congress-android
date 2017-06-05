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
        public StateOrTerritory State;
        public Gender Gender;

        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }

        public ContactMethod OfficeAddress { get; set; }
        public ContactMethod OfficeNumber { get; set; }

        public ContactMethod Email { get; set; }
        public ContactMethod FacebookId { get; set; }
        public ContactMethod TwitterId { get; set; }
        public ContactMethod YouTubeId { get; set; }
        public ContactMethod Website { get; set; }
        public ContactMethod ContactSite { get; set; }

        public int TotalVotes { get; set; }
        public int MissedVotesPercent { get; set; }
        public int VotesWithPartyPercent { get; set; }

        public string Senority { get; set; }
        public string BioguideId { get; set; }

        public string FullName()
        {
            return string.Format("{0} {1}{2}",
                FirstName,
                string.IsNullOrWhiteSpace(MiddleName)
                    ? string.Empty
                    : $"{MiddleName} ",
                LastName);
        }

  
        public string FormalAddressTitle()
        {
            switch (Chamber)
            {
                case LegislativeBody.Senate:
                    return $"Senator {LastName}";
                case LegislativeBody.House:
                    return string.Format("{0} {1}",
                        Gender == Gender.Female
                            ? "Congresswoman"
                            : "Congressman",
                        LastName);
                case LegislativeBody.Unknown:
                default:
                    return string.Empty;
            }
        }
    }
}
