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
        private int _defautlResultsPerPage = 40;

        public  BillManager(IMyLogger logger)
        {
            _billSvc = new BillSvc(logger);
        }

        public bool IsThereMoreResultsForLastCall()
        {
            return false;
            //TODO RM: Implement
            //return _billSvc.IsThereMoreResults();
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page)
        {
            return GetBillsSponsoredbyLegislator(legislatorBioguideId, page, _defautlResultsPerPage);
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            var billResult = new List<Bill>();

            if (resultsPerPage < 1)
                resultsPerPage = _defautlResultsPerPage;

            var ibillsResult = _billSvc.GetBillsIntroducedByLegislator(legislatorBioguideId, page, resultsPerPage);


            //TODO RM: <<<<<<CONTINUE HERE: Test is this works>>>>>>
            foreach (var ibill in ibillsResult)
                billResult.Add(Bill.TransformToBill(ibill));

            return billResult;
        }

        public List<Bill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page)
        {
            return GetBillsCosponsoredbyLegislator(legislatorBioguideId, page, _defautlResultsPerPage);
        }

        public List<Bill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (resultsPerPage < 1)
                resultsPerPage = _defautlResultsPerPage;

            return _billSvc.GetBillsCosponsoredbyLegislator(legislatorBioguideId, page, resultsPerPage);
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
