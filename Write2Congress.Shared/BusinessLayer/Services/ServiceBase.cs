using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public abstract class ServiceBase
    {
        protected string sunlightApiKey = "";
        protected string sunlighApiBaseUri = @"https://congress.api.sunlightfoundation.com/";

        protected HttpClient CreateSunlightHttpClient()
        {
            return CreateHttpClient(sunlighApiBaseUri);
        }

        private HttpClient CreateHttpClient(string baseUri)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


    }
}
