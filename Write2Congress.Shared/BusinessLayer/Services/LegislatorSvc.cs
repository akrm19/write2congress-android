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
        private UnitedStatesIoApi _usIoApiSvc;

        public LegislatorSvc(IMyLogger logger) : base(logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _usIoApiSvc = new UnitedStatesIoApi(logger);
        }

        public byte[] GetLegislatorPortrait2(string legislatorBioID)
        {
            //Possible options: 450x550 and original (typically 675x825, but can vary)
            var imageSize = "225x275";
            var uri = $@"http://theunitedstates.io/images/congress/{imageSize}/{legislatorBioID}.jpg";

            //byte[] result;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.MaxResponseContentBufferSize = 256000;

                    //return httpClient.GetByteArrayAsync(uri);
                    return httpClient.GetByteArrayAsync(uri).Result;
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
            //https://api.propublica.org/congress/v1/{congress}/{chamber}/members.json
            var senateMembersUri = "115/senate/members.json";
            var houseMembersUri = "115/house/members.json";

            var houseMembers = GetLegislatorsBase<CongressMembersResult.Rootobject>(houseMembersUri, _congressApiSvc).Result;
            var senators = GetLegislatorsBase<SenateMembersResult.Rootobject>(senateMembersUri, _congressApiSvc).Result;

            houseMembers.AddRange(senators);


            return houseMembers;
        }

        public List<ILegislator> GetLegislatorsFromUsIoApi()
        {
            var usIoLegislators = "congress-legislators/legislators-current.json";
            Func<string, string> updateResult = r => string.Format("{{\"results\":{0}}}", r);

            var legislators = new List<ILegislator>();

            try
            {
                var legislatorsResults = GetMemberResults<DomainModel.ApiModels.UnitedStatesIo.CongressLegislatorsResult.Rootobject>(usIoLegislators, _usIoApiSvc, updateResult).Result;
                legislators = (legislatorsResults as ILegislatorResult).GetLegislatorsResult();
            }
            catch(Exception e)
            {
                _logger.Error("Error occurred retrieving Legislators from UnitedStatesIo API", e);
            }

            return legislators;
        }

        internal List<ICommittee> GetLegislatorsCommitteesFromProPublica(string bioId)
        {
            if (string.IsNullOrWhiteSpace(bioId))
                return null;

            //https://api.propublica.org/congress/v1/members/{member-id}.json
            var getMembersUri = $"members/{bioId}.json";

            try
            {
                var committees  = GetLegislatorBase<DomainModel.ApiModels.ProPublica.SingleLegislatorResult.Rootobject>(getMembersUri, _congressApiSvc).Result;
                return committees.OrderBy(c => c.Name).ToList();
            }
            catch(Exception e)
            {
                _logger.Error($"Error occured retrieving legislator with bioId {bioId}", e);
                return null;
            }

        }

        private async Task<List<ICommittee>> GetLegislatorBase<T1>(string legislatorsUri, ApiBase apiSvc) where T1 : ICommitteeResult
        {
            try
            {
                var client = apiSvc.CreateHttpClient();
                var response = client.GetAsync(legislatorsUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    //var responseText = await response.Content.ReadAsStringAsync();

                    var results = JsonConvert.DeserializeObject<T1>(responseText);

                    var legislator = results.GetCommitteeResult();

                    return legislator;
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

        private async Task<List<ILegislator>> GetLegislatorsBase<T>(string legislatorsUri, ApiBase apiSvc) where T : ILegislatorResult
        {
            var legislators = new List<ILegislator>();

            try
            {
                var client = apiSvc.CreateHttpClient();

                //TODO RM (low priority) Ensure this is async
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
