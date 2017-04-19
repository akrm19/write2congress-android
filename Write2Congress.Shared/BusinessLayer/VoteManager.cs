using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer
{
    public class VoteManager
    {
        private VoteSvc _voteSvc;
        private int _defaultResultsPerPage = 40;

        public VoteManager(IMyLogger logger)
        {
            _voteSvc = new VoteSvc(logger);
        }

        public List<Vote> GetLegislatorVotes(string legislatorBioguideId, int page)
        {
            return GetLegislatorVotes(legislatorBioguideId, page, _defaultResultsPerPage);
        }

        public List<Vote> GetLegislatorVotes(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (resultsPerPage < 1)
                resultsPerPage = _defaultResultsPerPage;

            var votes = _voteSvc.GetVotesByLegislator(legislatorBioguideId, page, resultsPerPage);

            return votes;
        }
    }
}
