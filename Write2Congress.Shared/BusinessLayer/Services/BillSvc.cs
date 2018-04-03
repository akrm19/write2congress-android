using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class BillSvc : ServiceBase
    {
        private static int _defaultResultsPage = 20;
        private static string _billsBase = "bills?";
        private static string _fields = "&fields=bill_id,bill_type,number,congress,chamber,introduced_on,last_vote_at,official_title,short_title,popular_title,nicknames,summary,summary_short,urls,history,last_action,cosponsor_ids,withdrawn_cosponsor_ids,upcoming";
        private static string _perPage = "&per_page=";
        private static string _page = "&page=";
        private Util _util;

        private ProPublicaCongressApi _congressApiSvc;

        public BillSvc(IMyLogger logger) : base(logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _util = new Util(logger);
        }


        //TODO RM: Update method call, the resutls per page has to be 20 
        public List<IBill> GetBillsIntroducedByLegislator(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            page = page <= 1
                ? page = 0
                : page - 1;

            var resultsOffset = page * resultsperPageForSvc;

            //https://api.propublica.org/congress/v1/members/{member-id}/bills/{type}.json
            //var query = "sponsor_id__in=" + legislatorBioguideId;
            var query = $"members/{legislatorBioguideId}/bills/introduced.json?offset={resultsOffset}";

            //var bills = GetBillsFromQuery222(query, page, resultsPerPage);
            //
            //return bills;

            //Seems like this is no longer needed, as latest returns from ProPublica contain result
            //Func<string, string> updateResult = r => string.Format("{{\"results\":{0}}}", r);

            var bills = new List<IBill>();

            try
            {
                var legislatorsResults = GetMemberResults<DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(query, _congressApiSvc).Result;
                bills = (legislatorsResults as IBillResult).GeBillResult();
            }
            catch (Exception e)
            {
                _logger.Error("Error occurred retrieving Legislators from UnitedStatesIo API", e);
            }

            return bills.OrderByDescending(b => b.DateIntroduced).ToList();
        }

        //private List<IBill> GetBillsFromQuery222(string query, int page, int resultsPerPage)
        private List<IBill> GetBillsFromQuery222(string query, ApiBase apiSvc)
        {

            var bills = new List<IBill>();

            try
            {
                //var uri = CreateUri(query, page, resultsPerPage);
                //var result = GetTypeAsync<SunlightBillResult.Rootobject>(uri).Result;
                //PopulatePageInfoAndTotalResultCount(result);
                //
                //bills = _util.BillsFromSunlightBillResult(result);

                return bills;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

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
                //var result = GetTypeAsync<SunlightBillResult.Rootobject>(uri).Result;
                //PopulatePageInfoAndTotalResultCount(result);

                //bills = _util.BillsFromSunlightBillResult(result);

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
