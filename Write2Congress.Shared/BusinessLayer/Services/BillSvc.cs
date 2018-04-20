﻿using System;
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

        public List<IBill> GetBillsIntroducedByLegislator(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //https://api.propublica.org/congress/v1/members/{member-id}/bills/{type}.json
            //var query = "sponsor_id__in=" + legislatorBioguideId;
            var query = $"members/{legislatorBioguideId}/bills/introduced.json";
            query = CreateUri(query, page, resultsperPageForSvc);

            var bills = GetBillsFromQuery(query);

            return bills.OrderByDescending(b => b.DateIntroduced).ToList();
        }

        public ApiResultWithMoreResultIndicator<IBill> GetBillsIntroducedByLegislator2(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //https://api.propublica.org/congress/v1/members/{member-id}/bills/{type}.json
            //var query = "sponsor_id__in=" + legislatorBioguideId;
            var query = $"members/{legislatorBioguideId}/bills/introduced.json";
            //query = CreateUri(query, page, resultsperPageForSvc);

            //var bills = GetBillsFromQuery(query);

            //return bills.OrderByDescending(b => b.DateIntroduced).ToList();

            var results = GetApiResultFromQuery<IBill, DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(query, page, resultsperPageForSvc);

            return results;
        }

        public List<IBill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //Original query format
            //https://api.propublica.org/congress/v1/members/{legislatorBioguideId}/bills/cosponsored.json
            var query = $"members/{legislatorBioguideId}/bills/cosponsored.json";
            query = CreateUri(query, page, resultsperPageForSvc);

            var bills = GetBillsFromQuery(query);

            return bills.OrderByDescending(b => b.DateIntroduced).ToList();
        }

        public ApiResultWithMoreResultIndicator<IBill> GetBillsCosponsoredbyLegislator2(string legislatorBioguideId, int page = 1, int resultsperPageForSvc = 20)
        {
            //Original query format
            //https://api.propublica.org/congress/v1/members/{legislatorBioguideId}/bills/cosponsored.json
            var query = $"members/{legislatorBioguideId}/bills/cosponsored.json";
            //query = CreateUri(query, page, resultsperPageForSvc);

            var apiResult = GetApiResultFromQuery<IBill, DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(query, page, resultsperPageForSvc);


            return apiResult;
            //var bills = GetBillsFromQuery(query);

            //return bills.OrderByDescending(b => b.DateIntroduced).ToList();
        }

        private string CreateUri(string query, int page, int resultsPerPage = _defaultResultsPage)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException($"Error: Cannot retrieve Bills for legislator because of empty query: {query}");

            page = page <= 1
                ? page = 0
                : page - 1;

            var resultsOffset = page * resultsPerPage;

            var uri = $"{query}?offset={resultsOffset}";

            return uri;
        }

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

        protected ApiResultWithMoreResultIndicator<T>  GetApiResultFromQuery<T, T2>(string query, int page, int expectedResultsPerPage = _defaultResultsPage) where T2: class, IServiceResult<T>
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Error: Cannot retrieve results due to an invalid or empty query");
            
            try
            {
                //update query
                query = CreateUri(query, page, expectedResultsPerPage);

                //var votesResults = GetMemberResults<DomainModel.ApiModels.ProPublica.VotesResult.Rootobject>(query, _congressApiSvc).Result;
                var votesResults = GetMemberResults<T2>(query, _congressApiSvc).Result;
                var results = (votesResults as IServiceResult<T>).GetResults();

                var isThereMoreResults = true;

                if (results.Count < expectedResultsPerPage)
                    isThereMoreResults = false;
                else
                    isThereMoreResults = true;


                var apiResults = new ApiResultWithMoreResultIndicator<T>(results, isThereMoreResults);

                return apiResults;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return null;
        }
    }
}
