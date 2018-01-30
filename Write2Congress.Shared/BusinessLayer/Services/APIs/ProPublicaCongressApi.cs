using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services.APIs
{
    public class ProPublicaCongressApi : ApiBase
    {
        /*
         * TODO RM:
         * Make ServiceBase more generic, so it can be
         * used by several different APIs. Create ProPublicServiceAPI
         * that adds APi key to calls and ads version and also updates 
         * current congress (curennt congress is 115, but that will change)
         * https://projects.propublica.org/api-docs/congress-api/#requests
        */
        private string _congressApiKey = "PCuqpNcKuIudgm6zslBcTsRA24v7ARcsRN94aOa5";
        private string _congressApiUrl = "https://api.propublica.org/congress/{0}/";
        //TODO RM: Add mechanism to update API version
        private static string _apiVersion = "v1";

        public ProPublicaCongressApi(IMyLogger logger)
        {
            SetLogger(logger);
        }

        protected override string GetApiBaseUrl()
        {
            return string.Format(_congressApiUrl, _apiVersion);
        }

        protected override void ModifyHttpClientIfNeeded(HttpClient httpClient)
        {
            AddProPublicaApiKey(httpClient);
        }

        private void AddProPublicaApiKey(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _congressApiKey);
        }

        //TODO RM: Make this dynamic so it get the current congress number for the current date time
        protected string GetCurrentCongressNum()
        {
            return "115";
        }
    }
}
