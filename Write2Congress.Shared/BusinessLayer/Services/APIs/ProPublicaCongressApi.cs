using System;
using System.Net.Http;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services.APIs
{
    public class ProPublicaCongressApi : ApiBase
    {
        private string _congressApiKey = "PCuqpNcKuIudgm6zslBcTsRA24v7ARcsRN94aOa5";
        private string _congressApiUrl = "https://api.propublica.org/congress/{0}/";
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

        //TODO RM (low priority): Make this dynamic so it get the current congress number for the current date time
        protected string GetCurrentCongressNum()
        {
            var currentDate = DateTime.Now;

            if (currentDate < new DateTime(2019, 01, 04))
                return "115";
            else if (currentDate < new DateTime(2021, 01, 04))
                return "116";
            else if (currentDate < new DateTime(2023, 01, 04))
                return "117";
            else if (currentDate < new DateTime(2025, 01, 04))
                return "118";
            else
                return "119";
        }
    }
}
