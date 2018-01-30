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
            public Cosponsors_By_Party cosponsors_by_party { get; set; }
            public int cosponsors { get; set; }
            public string bill_id { get; set; }
            public string bill_type { get; set; }
            public string bill_uri { get; set; }
            public string committees { get; set; }
            public string congress { get; set; }
            public string congressdotgov_url { get; set; }
            //"2015-12-18",
            public string enacted { get; set; }
            public string govtrack_url { get; set; }
            public string gpo_pdf_uri { get; set; }
            public string house_passage { get; set; }
            public string introduced_date { get; set; }
            public string last_vote { get; set; }
            public string latest_major_action { get; set; }
            public string latest_major_action_date { get; set; }
            public string number { get; set; }
            public string primary_subject { get; set; }
            public string senate_passage { get; set; }
            public string short_title { get; set; }
            public string sponsor_id { get; set; }
            public string sponsor_name { get; set; }
            public string sponsor_party { get; set; }
            public string sponsor_state { get; set; }
            public string sponsor_title { get; set; }
            public string sponsor_uri { get; set; }
            public string summary { get; set; }
            public string summary_short { get; set; }
            public string title { get; set; }
            //"2015-12-18",
            public string vetoed { get; set; }


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
            string IBill.Id
            {
                get { return bill_id ?? string.Empty; }
                set { bill_id = value; }
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
            string IBill.TitleOfficial
            {
                get { return title ?? string.Empty; }
                set { title = value; }
            }
            string IBill.TitleShort
            {
                get { return short_title ?? string.Empty; }
                set { short_title = value; }
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
            BillStatusKind IBill.Status
            {
                get { return DataTransformationUtil.BillStatusFromProPublicaBill(this); }
                set { }
            }
            BillStatus IBill.LastAction
            {
                get { return DataTransformationUtil.LastBillActionFromProPublicaBill(this); }
                set { }
            }
            StateOrTerritory IBill.SponsorState
            {
                get { return DataTransformationUtil.GetStateOrTerritoryFromSunlight(sponsor_state); }
                set { }
            }
        }

        public class Cosponsors_By_Party
        {
            public int D { get; set; }
            public int R { get; set; }
        }
    }
}
