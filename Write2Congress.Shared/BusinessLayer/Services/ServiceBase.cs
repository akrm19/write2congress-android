using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public abstract class ServiceBase
    {
        protected string sunlightApiKey = "";
        protected static string sunlighApiBaseUri = "https://congress.api.sunlightfoundation.com/";
        protected IMyLogger _logger;
        
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

        public static async Task<T> GetTypeAsync<T>(string resourceUrl)
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
                    //TODO Add logging and handling
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
