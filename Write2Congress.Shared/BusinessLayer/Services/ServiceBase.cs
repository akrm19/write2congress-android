using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;
using static Write2Congress.Shared.DomainModel.SunlightBaseResult;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public abstract class ServiceBase
    {
        protected string sunlightApiKey = "";
        protected static string sunlighApiBaseUri = "https://congress.api.sunlightfoundation.com/";
        protected IMyLogger _logger;
        protected Page pageForLastResult = new Page();
        protected int totalResultCount = 0;
        protected static int defaultResultsPage = 40;

        protected void SetLogger(IMyLogger logger)
        {
            _logger = logger;
        } 

        protected static HttpClient CreateSunlightHttpClient()
        {
            return CreateHttpClient(sunlighApiBaseUri);
        }

        private static HttpClient CreateHttpClient(string baseUri)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<T> GetTypeAsync<T>(string resourceUrl)
        {
            T results = default(T);

            try
            {
                var client = CreateSunlightHttpClient();
                //var response = await client.GetAsync(resourceUrl);
                var response = client.GetAsync(resourceUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<T>(responseText);
                }
                else
                {
                    _logger.Error($"Error making call to {resourceUrl}");
                    return default(T);
                }
            }
            catch (Exception e)
            {
                var eMessage = e.Message;
            }

            return results;
        }

        protected void PopulatePageInfoAndTotalResultCount(ISunlightResult result)
        {
            totalResultCount = result.count ?? 0;
            pageForLastResult.count = result.page.count ?? 0;
            pageForLastResult.page = result.page.page ?? 1;
            pageForLastResult.per_page = result.page.per_page ?? 0;
        }

        protected int GetTotalResultsForLastCall()
        {
            return totalResultCount;
        }

        protected int GetCurrentPage()
        {
            return pageForLastResult.page ?? 1;
        }

        protected int GetResultsInCurrentPage()
        {
            return pageForLastResult.count ?? 0;
        }

        protected int GetExpectedResultPerPage()
        {
            return pageForLastResult.per_page ?? 0;
        }

        protected int GetMaxPossibleResultInCurrentPage()
        {
            if (GetCurrentPage() <= 1)
                return GetResultsInCurrentPage();

            var previousPage = GetCurrentPage() - 1;
            var resultsFromPreviousPages = previousPage * GetExpectedResultPerPage();

            return resultsFromPreviousPages + GetResultsInCurrentPage();
        }

        public bool IsThereMoreResults()
        {
            if (GetTotalResultsForLastCall() == 0 || GetResultsInCurrentPage() == 0 
                || GetExpectedResultPerPage() == 0)
                return false;

            if (GetResultsInCurrentPage() < GetExpectedResultPerPage())
                return false;

            else if (GetMaxPossibleResultInCurrentPage() == GetTotalResultsForLastCall())
                return false;

            return GetMaxPossibleResultInCurrentPage() < GetTotalResultsForLastCall();
        }
    }
}
