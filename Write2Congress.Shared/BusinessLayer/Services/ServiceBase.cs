using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class ServiceBase
    {
        protected IMyLogger _logger;

        public ServiceBase(IMyLogger logger)
        {
            _logger = logger;
        }


        //protected async Task<T> GetMemberResults<T>(string legislatorsUri, ApiBase apiSvc) where T : class
        //{
        //    return GetMemberResults<T>(legislatorsUri, apiSvc, null).Result;
        //}
            
        protected async Task<T> GetMemberResults<T>(string legislatorsUri, ApiBase apiSvc, Func<string, string> actionToPerformOnJsonOutput = null) where T : class
        {
            try
            {
                var client = apiSvc.CreateHttpClient();

                //TODO Ensure this is async
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
    }
}
