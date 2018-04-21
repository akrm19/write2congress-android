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
            .AppendLine($"{dateIntroduced}: {bill.DateIntroduced.ToString("d")}")
            .AppendLine()
            .AppendLine($"{conSponsors}: {bill.CosponsorsCount}")
            .AppendLine();

            if (!string.IsNullOrWhiteSpace(bill.BillStatus.StatusText))
            {
                text.AppendLine($"{billStatus}: {bill.BillStatus.StatusText}")
                .AppendLine();

                if(bill.DateOfLastVote != DateTime.MinValue)
                    text.AppendLine($"{statusDate}: {bill.DateOfLastVote.ToString("d")}").AppendLine();
            }

            if(!string.IsNullOrWhiteSpace(bill.LastAction.Text) && bill.LastAction.Date != DateTime.MinValue)
            {
                text.AppendLine($"Last Action: {bill.LastAction.Text}").AppendLine();

                if (bill.LastAction.Date != DateTime.MinValue)
                    text.AppendLine($"Last Action Date: {bill.LastAction.Date.ToString("d")}").AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(bill.Summary))
                text.AppendLine($"{summary}: {bill.Summary}").AppendLine();

            return text.ToString();
        }
    }
}
