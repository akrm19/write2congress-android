using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class CommitteeSvc : SunlightApi
    {
        //private static string _committeesForLegislatorsUri = "committees?member_ids={0}&fields=name,committee_id,chamber,url,office,subcommittee,phone&per_page=all";

        /*
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

                var uri = string.Format(_committeesForLegislatorsUri, legislatorBioguideId);
                var result = GetTypeAsync<SunlightCommitteeResult.Rootobject>(uri).Result;
                PopulatePageInfoAndTotalResultCount(result);

                committees = Util.CommitteesFromSunlightCommitteeResult(result);
            }
            catch (Exception e)
            {
                _logger.Error($"Error occurred retriving Committees for legislator {legislatorBioguideId}.", e);
            }

            return committees;
        }
        */
    }
}
