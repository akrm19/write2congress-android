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
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.ApiModels.ProPublica;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class LegislatorSvc : ServiceBase
    {
        private ProPublicaCongressApi _congressApiSvc;

        public LegislatorSvc(IMyLogger logger) : base(logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
        }

        /*
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
        */

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

        public List<ILegislator> GetAllAlegislators()
        {
            var senateMembersUri = "115/senate/members.json";
            var houseMembersUri = "115/house/members.json";


            var houseMembers = GetLegislatorsBase<CongressMembersResult.Rootobject>(houseMembersUri, _congressApiSvc).Result;
            var senators = GetLegislatorsBase<SenateMembersResult.Rootobject>(senateMembersUri, _congressApiSvc).Result;

            houseMembers.AddRange(senators);

            return houseMembers;
        }

        private async Task<List<ILegislator>> GetLegislatorsBase<T>(string legislatorsUri, ApiBase apiSvc) where T : ILegislatorResult
        {
            var legislators = new List<ILegislator>();

            try
            {
                var client = apiSvc.CreateHttpClient();

                //TODO Ensure this is async
                var response = client.GetAsync(legislatorsUri).Result;
                //var response = await client.GetAsync(legislatorsByZipUri); //TODO Find out why this fails
                //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async


                if (response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    //var responseText = await response.Content.ReadAsStringAsync();

                    var results = JsonConvert.DeserializeObject<T>(responseText);

                    legislators = results.GetLegislatorsResult();
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
