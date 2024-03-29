﻿using System;
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
    }
}
