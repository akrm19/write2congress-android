using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services.APIs;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class VoteSvc : ServiceBase
    {
        private Util _util;
        private ProPublicaCongressApi _congressApiSvc;

        public VoteSvc(IMyLogger logger) : base (logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _util = new Util(logger);
        }

        public ApiResultWithMoreResultIndicator<IVote> GetVotesByLegislator(string legislatorBioguideId, int page, int expectedResultsPerPage)
        {
            if (string.IsNullOrWhiteSpace(legislatorBioguideId))
                throw new ArgumentException("Error: Cannot retrieve Votes for legislator due to an invalid or empty BioguideId");

            //Original query format
            //https://api.propublica.org/congress/v1/members/{member-id}/votes.json
            var query = $"members/{legislatorBioguideId}/votes.json";

            var results = GetApiResultFromQuery<IVote, DomainModel.ApiModels.ProPublica.VotesResult.Rootobject>(_congressApiSvc, query, page, expectedResultsPerPage);

            return results;
        }
    }
}
