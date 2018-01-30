using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IBill
    {
        //"s1804-115",
        string Id { get; set; }
        //"S.1804"
        string BillNumber { get; set; }
        //"https://api.propublica.org/congress/v1/115/bills/s1804.json"
        string BillUri { get; set; }
        //"A bill to establish a Medicare-for-all health insurance program."
        string TitleOfficial { get; set; }
        //"Medicare for All Act of 2017",
        string TitleShort { get; set; }
        string Summary { get; set; }
        string Committees { get; set; }
        //string LastAction { get; set; }
        string SponsorBioId { get; set; }
        string SponsorName { get; set; }
        StateOrTerritory SponsorState { get; set; }
        //"https://api.propublica.org/congress/v1/members/S000033.json"
        string SponsorUri { get; set; }
        //"https://www.govtrack.us/congress/bills/115/s2165"
        string CongressDotGovUrl { get; set; }
        //"https://www.govtrack.us/congress/bills/115/s2165",
        string GovTrackUrl { get; set; }
        DateTime DateIntroduced { get; set; }
        DateTime DateLastVoted { get; set; }
        int NumberOfCoSponsors { get; set; }
        BillStatusKind Status { get; set; }
        BillStatus LastAction { get; set; }
    }

    public interface IBillStatus
    {
        string Text { get; set; }
        DateTime Date { get; set; }
    }
}
