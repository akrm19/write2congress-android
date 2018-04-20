using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel.Interface;
using Write2Congress.Shared.DomainModel;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class ServiceBase
    {
        protected IMyLogger _logger;

        public ServiceBase(IMyLogger logger)
        {
            _logger = logger;
        }
            
        protected async Task<T> GetMemberResults<T>(string legislatorsUri, ApiBase apiSvc, Func<string, string> actionToPerformOnJsonOutput = null) where T : class
        {
            try
            {
                var client = apiSvc.CreateHttpClient();

                //TODO RM (Low Priority) Ensure this is async
                var response = client.GetAsync(legislatorsUri).Result;
                //var response = await client.GetAsync(legislatorsByZipUri); //TODO Find out why this fails
                //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async


                if (response.IsSuccessStatusCode)
                {
                    //var responseText = await response.Content.ReadAsStringAsync();
                    var responseText = response.Content.ReadAsStringAsync().Result;

                    if(actionToPerformOnJsonOutput != null)
                        responseText = actionToPerformOnJsonOutput(responseText);

                    var results = JsonConvert.DeserializeObject<T>(responseText);

                    return results;
                }
                else
                {
                    _logger.Error($"Error occurred retrieving legislators using URI: {legislatorsUri}");
                }
            }
            catch (Exception e)
            {
                _logger.Error("Error retrieving legislators.", e);
            }

            return null;
        }

        protected ApiResultWithMoreResultIndicator<T> GetApiResultFromQuery<T, T2>(ApiBase apiSvc, string query, int page, int expectedResultsPerPage) where T2 : class, IServiceResult<T>
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Error: Cannot retrieve results due to an invalid or empty query");

            try
            {
                //update query
                query = CreateUriForProPublica(query, page, expectedResultsPerPage);

                //var votesResults = GetMemberResults<DomainModel.ApiModels.ProPublica.VotesResult.Rootobject>(query, _congressApiSvc).Result;
                var votesResults = GetMemberResults<T2>(query, apiSvc).Result;
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

        protected string CreateUriForProPublica(string query, int page, int resultsPerPage)
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
    }
}
