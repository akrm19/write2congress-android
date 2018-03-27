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
        string BillId { get; set; }

        //"S.1804"
        string BillNumber { get; set; }

        //"https://api.propublica.org/congress/v1/115/bills/s1804.json"
        string BillUri { get; set; }

        /// <summary>
        /// The type for this bill. For the bill “H.R. 4921”, the bill_type represents the 
        /// “H.R.” part. Bill types can be: hr, hres, hjres, hconres, s, sres, sjres, sconres.
        /// </summary>
        BillType Type { get; }
       
        LegislativeBody Chamber { get; set; }

        //"Senate Health, Education, Labor, and Pensions Committee"
        string Committees { get; set; }
       
        //"115",
        int Congress { get; set; }
        
        //"https://www.govtrack.us/congress/bills/115/s2165"
        string CongressDotGovUrl { get; set; }

        int CoSponsorCount { get; }

        DateTime DateIntroduced { get; set; }

        DateTime DateLastVoted { get; set; }
        
        //"https://www.govtrack.us/congress/bills/115/s2165",
        string GovTrackUrl { get; set; }
        
        BillAction LastAction { get; set; }

        int NumberOfCoSponsors { get; set; }

        string SponsorBioId { get; set; }
        
        //"Bernard Sanders",
        string SponsorName { get; set; }

        StateOrTerritory SponsorState { get; set; }
        
        //"https://api.propublica.org/congress/v1/members/S000033.json"
        string SponsorUri { get; set; }

        BillStatus Status { get; set; }
        
        //Longer text 
        string Summary { get; set; }
        
        //Title of the bill (like official & short title)
        BillTitles Titles { get; }
    }

    public interface IBillStatus
    {
        string Text { get; set; }
        DateTime Date { get; set; }
    }
}
