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
    //https://congress.api.sunlightfoundation.com/votes?voter_ids.M001153__exists=true&fields=roll_id,chamber,number,year,congress,voted_at,vote_type,roll_type,question,required,result,source,bill_id,bill,nomination,nomination_id,breakdown,voter_ids.M001153&per_page=50&page=3
    
    public class VoteSvc : ServiceBase
    {
        private Util _util;
        private ProPublicaCongressApi _congressApiSvc;

        public VoteSvc(IMyLogger logger) : base (logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _util = new Util(logger);
        }

        public ApiResultWithMoreResultIndicator<IVote> GetVotesByLegislator2(string legislatorBioguideId, int page, int expectedResultsPerPage)
        {
            if (string.IsNullOrWhiteSpace(legislatorBioguideId))
                throw new ArgumentException("Error: Cannot retrieve Votes for legislator due to an invalid or empty BioguideId");

			//Original query format
			//https://api.propublica.org/congress/v1/members/{member-id}/votes.json
			var query = $"members/{legislatorBioguideId}/votes.json";
			query = AppendPageAndOffsetToQuery(query, page, expectedResultsPerPage);
   
            try
            {
                var votesResults = GetMemberResults<DomainModel.ApiModels.ProPublica.VotesResult.Rootobject>(query, _congressApiSvc).Result;
                var votes = (votesResults as IServiceResult<IVote>).GetResults();

                var firstResult = votesResults.results.FirstOrDefault();
                var moreResults = false;

                if (firstResult == null)
                    moreResults = false;
                else if (votes.Count < expectedResultsPerPage)
                    moreResults = false;
                else
                    moreResults = true;


                var newResult = new ApiResultWithMoreResultIndicator<IVote>(votes, moreResults);

                return newResult;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return null;
        }

        /*
        public List<IVote> GetVotesByLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (string.IsNullOrWhiteSpace(legislatorBioguideId))
                throw new ArgumentException("Error: Cannot retrieve Votes for legislator due to an invalid or empty BioguideId");

            //Original query format
            //https://api.propublica.org/congress/v1/members/{member-id}/votes.json
            var query = $"members/{legislatorBioguideId}/votes.json";
            query = AppendPageAndOffsetToQuery(query, page, resultsPerPage);

            var votes = GetVotesFromQuery(query);

            return votes;
        }

        private List<IVote> GetVotesFromQuery(string query)
        {
            var votes = new List<IVote>();

            try
            {
                var votesResults = GetMemberResults<DomainModel.ApiModels.ProPublica.VotesResult.Rootobject>(query, _congressApiSvc).Result;
                votes = (votesResults as IVoteResult).GetVoteResult();

                return votes;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return votes;
        }
        */

        private string AppendPageAndOffsetToQuery(string query, int page, int resultsPerPage)
        {
            if (page < 0)
                page = 0;
			
            var resultsOffSet = page * resultsPerPage;

            var uri = $"{query}?offset={resultsOffSet}";

            return uri;
        }
    }
}
