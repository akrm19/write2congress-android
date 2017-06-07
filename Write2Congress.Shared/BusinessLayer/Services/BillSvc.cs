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
        private static string _billsBase = "bills?";
        private static string _fields = "&fields=bill_id,bill_type,number,congress,chamber,introduced_on,last_vote_at,official_title,short_title,popular_title,nicknames,summary,summary_short,urls,history,last_action,cosponsor_ids,withdrawn_cosponsor_ids,upcoming";
        private static string _perPage = "&per_page=";
        private static string _page = "&page=";
        private Util _util;

        public BillSvc(IMyLogger logger)
        {
            SetLogger(logger);
            _util = new Util(logger);
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            var query = "sponsor_id__in=" + legislatorBioguideId;
            var bills = GetBillsFromQuery(query, page, resultsPerPage);

            return bills;
        }

        public List<Bill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            //"cosponsor_ids__in=" + legislatorBioguideId;
            var query = "cosponsor_ids__all=" + legislatorBioguideId;
            var bills = GetBillsFromQuery(query, page, resultsPerPage);

            return bills;
        }

        private List<Bill> GetBillsFromQuery(string query, int page, int resultsPerPage)
        {
            var bills = new List<Bill>();

            try
            {
                var uri = CreateUri(query, page, resultsPerPage);
                var result = GetTypeAsync<SunlightBillResult.Rootobject>(uri).Result;
                PopulatePageInfoAndTotalResultCount(result);

                bills = _util.BillsFromSunlightBillResult(result);

                return bills;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return bills;
        }

        private string CreateUri(string query, int page, int resultsPerPage)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException($"Error: Cannot retrieve Bills for legislator because of invalid query: {query}");

            if (page < 1)
                throw new ArgumentException($"Error: Cannot retrieve Bills for legislator because of invalid page value: {page}");

            if (resultsPerPage < 1)
                resultsPerPage = _defaultResultsPage;

            var uri = string.Format("{0}{1}{2}{3}{4}",
                _billsBase,
                query,
                _fields,
                _perPage + resultsPerPage,
                _page + page.ToString());

            return uri;
        }
    }
}
