using System;
using System.Collections.Generic;
using System.Linq;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class BillSvc : ServiceBase
    {
		private const int _defaultResultsPage = 20;
        private Util _util;
        private ProPublicaCongressApi _congressApiSvc;

        public BillSvc(IMyLogger logger) : base(logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _util = new Util(logger);
        }

        public ApiResultWithMoreResultIndicator<IBill> GetBillsIntroducedByLegislator2(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //Query Format
            //https://api.propublica.org/congress/v1/members/{member-id}/bills/{type}.json
            var query = $"members/{legislatorBioguideId}/bills/introduced.json";

            var results = GetApiResultFromQuery<IBill, DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(_congressApiSvc, query, page, resultsperPageForSvc);

            return results;
        }

        public ApiResultWithMoreResultIndicator<IBill> GetBillsCosponsoredbyLegislator2(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //Original query format
            //https://api.propublica.org/congress/v1/members/{legislatorBioguideId}/bills/cosponsored.json
            var query = $"members/{legislatorBioguideId}/bills/cosponsored.json";

            var apiResult = GetApiResultFromQuery<IBill, DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(_congressApiSvc, query, page, resultsperPageForSvc);


            return apiResult;
        }

        //Not used, but left for reference
        private List<IBill> GetBillsFromQuery(string query)
        {
            var bills = new List<IBill>();

            try
            {
                var legislatorsResults = GetMemberResults<DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(query, _congressApiSvc).Result;
                bills = (legislatorsResults as IServiceResult<IBill>).GetResults();

                return bills;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return bills;
        }
    }
}
