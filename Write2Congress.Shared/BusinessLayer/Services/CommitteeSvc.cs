using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class CommitteeSvc : ServiceBase
    {
        private static string _allLegislatorsUri = "committees?member_ids={0}&per_page=all";

        public CommitteeSvc(IMyLogger logger)
        {
            SetLogger(logger);
        }

        public List<Committee> GetCommitteesForLegislator(string legislatorBioguideId)
        {
            var committees = new List<Committee>();

            try
            {
                if (string.IsNullOrWhiteSpace(legislatorBioguideId))
                {
                    _logger.Warn($"Error occurred retriving Committees for legislator. LegislatorBioguideId is null or empty");
                    return committees;
                }

                var uri = string.Format(_allLegislatorsUri, legislatorBioguideId);
                var result = GetTypeAsync<SunlightCommitteesByLegislatorResult.Rootobject>(uri).Result;
               
                committees = Util.CommitteesFromSunlightCommitteeResult(result);
            }
            catch (Exception e)
            {
                _logger.Error($"Error occurred retriving Committees for legislator {legislatorBioguideId}. Uri {_allLegislatorsUri}.", e);
            }

            return committees;
        }
    }
}
