using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel.Interface;
using static Write2Congress.Shared.DomainModel.SunlightBaseResult;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public abstract class SunlightApi : ApiBase
    {
        protected static string sunlighApiBaseUri = "https://api.propublica.org/congress/";      
        protected Page pageForLastResult = new Page();
        protected int totalResultCount = 0;
        protected static int defaultResultsPage = 40;
        
        protected override string GetApiBaseUrl()
        {
            return $"{sunlighApiBaseUri}/";
        }

        protected override void ModifyHttpClientIfNeeded(HttpClient httpClient)
        {

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
