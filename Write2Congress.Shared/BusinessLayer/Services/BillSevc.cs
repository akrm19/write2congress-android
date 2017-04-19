using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class BillSvc : ServiceBase
    {
        private static int _defaultResultsPage = 30;
        private static string _billsSponsorByLegislatorUri = "bills?sponsor_id__in={0}&fields=bill_id,bill_type,number,congress,chamber,introduced_on,last_vote_at,official_title,short_title,popular_title,nicknames,summary,summary_short,urls,history,last_action,cosponsor_ids,withdrawn_cosponsor_ids,upcoming&per_page={2}&page={1}";

        public BillSvc(IMyLogger logger)
        {
            SetLogger(logger);
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page, int? resultsPerPage = null)
        {
            var bills = new List<Bill>();

            if(string.IsNullOrWhiteSpace(legislatorBioguideId))
            {
                _logger.Error("Error: Cannot retrieve Bills for legislator because ID is null or empty");
                return bills;
            }

            if (page < 1)
            {
                _logger.Error($"Error: Cannot retrieve Bills for legislator {legislatorBioguideId} because of invalid page value: {page}");
                return bills;
            }

            try
            {
                var uri = string.Format(_billsSponsorByLegislatorUri, legislatorBioguideId, page, (resultsPerPage ?? _defaultResultsPage));
                var result = GetTypeAsync<SunlightBillResult.Rootobject>(uri).Result;

                bills = Util.BillsFromSunlightBillResult(result);

                return bills;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills sponsored by {legislatorBioguideId}", ex);
            }

            return bills;
        }
    }
}
