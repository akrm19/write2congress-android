using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface ILegislator
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        DateTime Birthday { get; set; }

        Party Party { get; set; }
        LegislativeBody Chamber { get; set; }
        StateOrTerritory State { get; set; }
        Gender Gender { get; set; }

        DateTime TermStartDate { get; set; }
        DateTime TermEndDate { get; set; }

        ContactMethod OfficeAddress { get; set; }
        ContactMethod OfficeNumber { get; set; }

        ContactMethod Email { get; set; }
        ContactMethod FacebookId { get; set; }
        ContactMethod TwitterId { get; set; }
        ContactMethod YouTubeId { get; set; }
        ContactMethod Website { get; set; }
        ContactMethod ContactSite { get; set; }

        int TotalVotes { get; set; }
        float MissedVotesPercent { get; set; }
        float VotesWithPartyPercent { get; set; }

        string Senority { get; set; }
        string BioguideId { get; set; }
    }
}
