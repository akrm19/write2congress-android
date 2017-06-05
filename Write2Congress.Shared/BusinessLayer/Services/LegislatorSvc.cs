using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class LegislatorSvc : ServiceBase
    {
        public LegislatorSvc(IMyLogger logger)
        {
            _logger = logger;
        }

        public Task<byte[]> GetLegislatorPortrait2(Legislator legislator)
        {
            //Possible options: 450x550 and original (typically 675x825, but can vary)
            var imageSize = "225x275";
            var uri = $@"http://theunitedstates.io/images/congress/{imageSize}/{legislator.BioguideId}.jpg";

            byte[] result;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.MaxResponseContentBufferSize = 256000;

                    return httpClient.GetByteArrayAsync(uri);
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Error encountured retrieving portrait from URL: {uri}. Error {e.ToString()}");
                return null;
            }
        }

        public async Task<byte[]> GetLegislatorPortrait(Legislator legislator)
        {
            //Possible options: 450x550 and original (typically 675x825, but can vary)
            var imageSize = "225x275";
            var uri = $@"http://theunitedstates.io/images/congress/{imageSize}/{legislator.BioguideId}.jpg";

            byte[] result;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    result = await httpClient.GetByteArrayAsync(uri);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.Error($"Error encountured retrieving portrait from URL: {uri}. Error {e.ToString()}");
                return null;
            }
        }

        public List<Legislator> GetAllAlegislators()
        {
            var allLegislatorsUri = "legislators?per_page=all";

            return GetLegislatorsBase(allLegislatorsUri).Result;
        }

        public async Task<List<Legislator>> GetLegislatorsByZipCode(string zipCode)
        {
            var legislatorsByZipUri = "legislators/locate?zip=" + zipCode;

            return GetLegislatorsBase(legislatorsByZipUri).Result;
        }

        private async Task<List<Legislator>> GetLegislatorsBase(string legislatorsUri)
        {
            var legislators = new List<Legislator>();

            try
            {
                var client = CreateSunlightHttpClient();

                // RestUrl = https://congress.api.sunlightfoundation.com/legislators/locate?zip={0}
                //TODO Ensure this is async
                var response = client.GetAsync(legislatorsUri).Result;

                //var response = await client.GetAsync(legislatorsByZipUri); //TODO Find out why this fails
                //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async


                if (response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    //var responseText = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<SunlightLegislatorResult.Rootobject>(responseText);

                    legislators = Util.LegislatorsFromSunlightLegislatorResult(results);
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

            return legislators;
        }
    }
}
