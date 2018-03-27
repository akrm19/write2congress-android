using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;
using static Write2Congress.Shared.DomainModel.ApiModels.ProPublica.BaseResult;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public class BillResult
    {
        public class Rootobject : BaseRootObject, IBillResult 
        {
            public Result[] results { get; set; }

            List<IBill> IBillResult.GeBillResult()
            {
                var bills = new List<IBill>();

                foreach (var billsResult in results.Where(r => r != null && r.bills.Count() > 0))
                    bills.AddRange(billsResult.bills);

                return bills;
            }
        }

        public class Result
        {
            public Bill[] bills { get; set; }
            public int num_results { get; set; }
            public int offset { get; set; }
            public string id { get; set; }
            public string member_uri { get; set; }
            public string name { get; set; }
        }

        public class Bill : IBill
        {
            public bool active { get; set; }
            /*
            "cosponsors_by_party": {
                "D": 3
            },
            */
            public Cosponsors_By_Party cosponsors_by_party { get; set; }
            public int cosponsors { get; set; }
            //"s1441-115"
            public string bill_id { get; set; }
            //"s",
            public string bill_type { get; set; }
            //"https://api.propublica.org/congress/v1/115/bills/s1441.json",
            public string bill_uri { get; set; }
            //"Senate Health, Education, Labor, and Pensions Committee",
            public string committees { get; set; }
            //"115",
            public string congress { get; set; }
            //"https://www.congress.gov/bill/115th-congress/senate-bill/1441",
            public string congressdotgov_url { get; set; }
            //"2015-12-18",
            public string enacted { get; set; }
            //"https://www.govtrack.us/congress/bills/115/s1441",
            public string govtrack_url { get; set; }
            public string gpo_pdf_uri { get; set; }
            //"2017-02-16"
            public string house_passage { get; set; }
            //"2017-06-26",
            public string introduced_date { get; set; }
            //"2017-03-21",
            public string last_vote { get; set; }
            //"latest_major_action": "Read twice and referred to the Committee on Health, Education, Labor, and Pensions."
            //"latest_major_action": "Became Public Law No: 115-20."
            public string latest_major_action { get; set; }
            //"latest_major_action_date": "2017-06-26"
            public string latest_major_action_date { get; set; }
            //"S.1441",
            public string number { get; set; }
            //"Health",
            public string primary_subject { get; set; }
            //"2017-03-21"
            public string senate_passage { get; set; }
            //"Providing for congressional disapproval under chapter 8 of title 5, United States Code, of the final rule of the Department of the Interior relating to \"Non-Subsistence Take of Wildlife, and Public Participation and Closure Procedures, on National Wildlif",
            public string short_title { get; set; }
            //"Y000033"
            public string sponsor_id { get; set; }
            //"Bernard Sanders",
            public string sponsor_name { get; set; }
            // "I",
            public string sponsor_party { get; set; }
            //"VT",
            public string sponsor_state { get; set; }
            //"Sen." | //"Rep.", |
            public string sponsor_title { get; set; }
            //"https://api.propublica.org/congress/v1/members/S000033.json",
            public string sponsor_uri { get; set; }
            public string summary { get; set; }
            public string summary_short { get; set; }
            //"A bill to provide funding for Federally Qualified Health Centers, the National Health Service Corps, Teaching Health Centers, and the Nurse Practitioner Residency Training program.",
            public string title { get; set; }
            //"2015-12-18",
            public string vetoed { get; set; }

            public class Cosponsors_By_Party
            {
                public int D { get; set; }
                public int R { get; set; }
            }

            #region IBill Implementation
            string IBill.BillNumber
            {
                get { return number ?? string.Empty ;}
                set { number = value; }
            }
            string IBill.BillUri
            {
                get { return bill_uri ?? string.Empty ; }
                set { bill_uri = value; }
            }
            string IBill.Committees
            {
                get { return committees ?? string.Empty; }
                set { committees = value; }
            }
            string IBill.CongressDotGovUrl
            {
                get { return congressdotgov_url ?? string.Empty ; }
                set { congressdotgov_url = value; }
            }
            string IBill.GovTrackUrl
            {
                get { return govtrack_url ?? string.Empty; }
                set { govtrack_url = value; }
            }
            string IBill.BillId
            {
                get { return bill_id ?? string.Empty; }
                set { bill_id = value; }
            }
            BillType IBill.Type
            {
                get { return DataTransformationUtil.BillTypeFromText(bill_type); }
            }
            string IBill.SponsorBioId
            {
                get { return sponsor_id ?? string.Empty ; }
                set { sponsor_id = value; }
            }
            string IBill.SponsorName
            {
                get { return sponsor_name ?? string.Empty; }
                set { sponsor_name = value; }
            }
            string IBill.SponsorUri
            {
                get { return sponsor_uri ?? string.Empty; ; }
                set { sponsor_uri = value; }
            }
            string IBill.Summary
            {
                get { return summary ?? string.Empty; ; }
                set { summary = value; }
            }
            BillTitles IBill.Titles
            {
                get
                {
                    var officialTitle = title ?? string.Empty;
                    var shortTitle = short_title ?? string.Empty;

                    return new BillTitles
                    {
                        OfficialTile = officialTitle,
                        ShortTitle = shortTitle,
                        PopularTitlePerLoc = string.Empty
                    };
                }
            }
            int IBill.NumberOfCoSponsors
            {
                get { return cosponsors; }
                set { cosponsors = value; }
            }
            DateTime IBill.DateIntroduced
            {
                get { return DataTransformationUtil.DateFromSunlightTime(introduced_date); }
                set { }
            }
            DateTime IBill.DateLastVoted
            {
                get { return DataTransformationUtil.DateFromSunlightTime(last_vote); }
                set { }
            }
            BillStatus IBill.Status
            {
                get { return DataTransformationUtil.BillStatusFromProPublicaBill(this); }
                set { }
            }
            BillAction IBill.LastAction
            {
                get { return DataTransformationUtil.LastBillActionFromProPublicaBill(this); }
                set { }
            }
            StateOrTerritory IBill.SponsorState
            {
                get { return DataTransformationUtil.GetStateOrTerritoryFromSunlight(sponsor_state); }
                set { }
            }

            LegislativeBody IBill.Chamber
            {
                get { return DataTransformationUtil.ChamberFromBillProPublica(this); }
                set { }
            }

            int IBill.Congress
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(congress))
                        return 0;

                    int congressNumber;
                    if (int.TryParse(congress, out congressNumber))
                        return congressNumber;
                    else
                        return 0;
                }
                set { }
            }

            int IBill.CoSponsorCount
            {
                get
                {
                    return cosponsors;
                }
            }


            #endregion
        }
    }
}
