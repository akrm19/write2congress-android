using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services.APIs
{
    public abstract class ApiBase
    {
        protected IMyLogger _logger;

        protected void SetLogger(IMyLogger logger)
        {
            _logger = logger;
        }

        protected abstract void ModifyHttpClientIfNeeded(HttpClient httpClient);

        protected abstract string GetApiBaseUrl();

        public HttpClient CreateHttpClient()
        {
            return CreateHttpClient(GetApiBaseUrl());
        }

        private HttpClient CreateHttpClient(string baseUri)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ModifyHttpClientIfNeeded(client);

            return client;
        }

        public async Task<T> GetTypeAsync<T>(string resourceUrl)
        {
            T results = default(T);

            try
            {
                var client = CreateHttpClient();
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
    }
}
