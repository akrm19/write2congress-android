using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer
{
    public class BillManager
    {
        private BillSvc _billSvc;
        private const int _defautlResultsPerPage = 20;

        public  BillManager(IMyLogger logger)
        {
            _billSvc = new BillSvc(logger);
        }

        public ApiResultWithMoreResultIndicator<Bill> GetBillsSponsoredbyLegislator2(string legislatorBioguideId, int page, int resultsPerPage = _defautlResultsPerPage)
        {
            var bills = new List<Bill>();

            var billsServiceResults = _billSvc.GetBillsIntroducedByLegislator2(legislatorBioguideId, page, resultsPerPage);

            foreach (var ibill in billsServiceResults.Results)
                bills.Add(Bill.TransformToBill(ibill));

            return new ApiResultWithMoreResultIndicator<Bill>(bills, billsServiceResults.IsThereMoreResults);
        }

        public ApiResultWithMoreResultIndicator<Bill> GetBillsCosponsoredbyLegislator2(string legislatorBioguideId, int page, int resultsPerPage = _defautlResultsPerPage)
        {
            var bills = new List<Bill>();

            var billsServiceResults = _billSvc.GetBillsCosponsoredbyLegislator2(legislatorBioguideId, page, resultsPerPage);

            foreach (var ibill in billsServiceResults.Results)
                bills.Add(Bill.TransformToBill(ibill));

            return new ApiResultWithMoreResultIndicator<Bill>(bills, billsServiceResults.IsThereMoreResults);
        }

        public ApiResultWithMoreResultIndicator<Bill>GetBillsIntroduced(int page, int resultsPerPage = _defautlResultsPerPage)
        {
            var bills = new List<Bill>();

            var billsServiceResults = _billSvc.GetBillsIntroduced(page, resultsPerPage);

            foreach (var ibill in billsServiceResults.Results)
                bills.Add(Bill.TransformToBill(ibill));

            return new ApiResultWithMoreResultIndicator<Bill>(bills, billsServiceResults.IsThereMoreResults);
        }

        public ApiResultWithMoreResultIndicator<Bill> GetBillsBySubject(string searchTerm, int page, int resultsPerPage = _defautlResultsPerPage)
        {
            var bills = new List<Bill>();

            var billsServiceResults = _billSvc.GetBillsIntroduced(page, resultsPerPage);

            foreach (var ibill in billsServiceResults.Results)
                bills.Add(Bill.TransformToBill(ibill));

            return new ApiResultWithMoreResultIndicator<Bill>(bills, billsServiceResults.IsThereMoreResults);
        }

        //TODO RM Finsih filtering logic
        public List<Bill> FilterBillsByQuery(List<Bill> billsToQuery, string query)
        {
            query = query.ToLower();

            //TODO RM: make sure title.popular & title.short are always availalbe 
            //or check for nulls
            return billsToQuery.Where(
                b => b.Summary.ToLower().Contains(query)
                || b.Titles.OfficialTile.ToLower().Contains(query)
                || b.Titles.PopularTitlePerLoc.ToLower().Contains(query)
                || b.Titles.ShortTitle.ToLower().Contains(query)
            ).OrderBy(br => br.GetDisplayTitle()).ToList();
        }


        public static string GetBillDetailedSummary(Bill bill)
        {
            var dateIntroduced = "Date Introduced";
            var conSponsors = "Co-sponsors";
            var billStatus = "Bill Status";
            var statusDate = "Status Date";
            var summary = "Bill Summary";

            var text = new StringBuilder();
            text.AppendLine(bill.GetDisplayTitleWithLabel())
                .AppendLine()
                .AppendLine($"{dateIntroduced}:")
                .AppendLine($"{bill.DateIntroduced.ToString("d")}")
                .AppendLine();

            if (!string.IsNullOrWhiteSpace(bill.BillStatus.StatusText))
            {
                text.AppendLine($"{billStatus}:")
                .AppendLine($"{bill.BillStatus.StatusText}")
                .AppendLine();

                if(bill.DateOfLastVote != DateTime.MinValue){
                    text.AppendLine($"{statusDate}:")
                    .AppendLine($"{bill.DateOfLastVote.ToString("d")}")
                    .AppendLine();
                }
            }

            if(!string.IsNullOrWhiteSpace(bill.LastAction.Text) && bill.LastAction.Date != DateTime.MinValue)
            {
                text.AppendLine($"Last Action:")
                .AppendLine($"{bill.LastAction.Text}")
                .AppendLine();

                if (bill.LastAction.Date != DateTime.MinValue)
                {
                    text.AppendLine($"Last Action Date:")
                    .AppendLine($"{bill.LastAction.Date.ToString("d")}")
                    .AppendLine();
                }
            }
                
            text.AppendLine($"{conSponsors}:")
                .AppendLine($"{bill.CosponsorsCount}")
                .AppendLine();

            if (!string.IsNullOrWhiteSpace(bill.Summary))
            {
                text.AppendLine($"{summary}:")
                .AppendLine($"{bill.Summary}")
                .AppendLine();
            }

            return text.ToString();
        }
    }
}
