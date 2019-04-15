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
            var currentCongress = Util.GetCurrentCongressNum();

            //https://api.propublica.org/congress/v1/{congress}/{chamber}/members.json
            var senateMembersUri = $"{currentCongress}/senate/members.json";
            var houseMembersUri = $"{currentCongress}/house/members.json";

            var houseMembers = GetLegislatorsBase2<CongressMembersResult.Rootobject>(houseMembersUri).Results;
            var senators = GetLegislatorsBase2<SenateMembersResult.Rootobject>(senateMembersUri).Results;

            houseMembers.AddRange(senators);


            return houseMembers.OrderBy(l => l.LastName).ToList();
        }

        private ApiResultWithMoreResultIndicator<ILegislator> GetLegislatorsBase2<T>(string membersUri) where T : class, IServiceResult<ILegislator>
        {
            var results = GetApiResultFromQuery<ILegislator, T>(_congressApiSvc, membersUri);

            return results;
        }

        public List<ILegislator> GetLegislatorsFromUsIoApi()
        {
            var usIoLegislators = "congress-legislators/legislators-current.json";
            Func<string, string> updateResult = r => string.Format("{{\"results\":{0}}}", r);

            var legislators = new List<ILegislator>();

            try
            {
                var legislatorsResults = GetMemberResults<DomainModel.ApiModels.UnitedStatesIo.CongressLegislatorsResult.Rootobject>(usIoLegislators, _usIoApiSvc, updateResult).Result;
                legislators = (legislatorsResults as IServiceResult<ILegislator>).GetResults();
            }
            catch(Exception e)
            {
                _logger.Error("Error occurred retrieving Legislators from UnitedStatesIo API", e);
            }

            return legislators;
        }

        public ApiResultWithMoreResultIndicator<ICommittee> GetLegislatorsCommitteesFromProPublica2(string legislatorBioguideId)
        {
            //https://api.propublica.org/congress/v1/members/{member-id}.json
            var getMembersUri = $"members/{legislatorBioguideId}.json";
            var results = GetApiResultFromQuery<ICommittee, SingleLegislatorResult.Rootobject>(_congressApiSvc, getMembersUri);

            return results;
        }
    }
}
