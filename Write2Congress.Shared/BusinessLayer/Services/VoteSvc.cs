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
        /*Old Sunlight params
        //https://congress.api.sunlightfoundation.com/voter_ids.M001153__exists=true
        private static string _votesBase = "votes?";
        private static string _fields = "&fields=chamber,year,voted_at,vote_type,question,required,result,source,bill_id,bill,nomination,nomination_id,breakdown";
        private static string _filedAppedix = ",voter_ids."; //",voter_ids.M001153" &per_page=50&page=3"
        private static string _perPage = "&per_page=";
        private static string _page = "&page=";
        */

        private Util _util;
        private ProPublicaCongressApi _congressApiSvc;


        public VoteSvc(IMyLogger logger) : base (logger)
        {
            _congressApiSvc = new ProPublicaCongressApi(logger);
            _util = new Util(logger);
        }

        public List<Vote> GetVotesByLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (string.IsNullOrWhiteSpace(legislatorBioguideId))
                throw new ArgumentException("Error: Cannot retrieve Votes for legislator due to an invalid or empty BioguideId");

            //Original query format
            //https://api.propublica.org/congress/v1/members/{member-id}/votes.json
            var query = $"members/{legislatorBioguideId}/votes.json";
            query = AppendPageAndOffsetToQuery(query, page, resultsPerPage);

            var votes = GetVotesFromQuery(query);

            return votes;

            /*old Sunlight logic
            var votes = new List<Vote>();

            try
            {
                var query = $"voter_ids.{legislatorBioguideId}__exists=true";
                var uri = CreateUri(query, legislatorBioguideId, page, resultsPerPage);

                var result = GetTypeAsync<SunlightVoteResult.Rootobject>(uri).Result;
                PopulatePageInfoAndTotalResultCount(result);

                votes = _util.VotesFromSunlightVoteResult(result, legislatorBioguideId);

                return votes;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occcured retriving votes for {legislatorBioguideId}", ex);
                return votes;
            }
            */
        }

        private List<Vote> GetVotesFromQuery(string query)
        {
            return new List<Vote>();
            //throw new NotImplementedException();

            //TODO RM: <<<CONTINUE HERE>>> Implement IVote and return a list of IVotes
            /*
            var bills = new List<IBill>();

            try
            {
                var legislatorsResults = GetMemberResults<DomainModel.ApiModels.ProPublica.BillResult.Rootobject>(query, _congressApiSvc).Result;
                bills = (legislatorsResults as IBillResult).GeBillResult();

                return bills;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: Could not retrieve Bills from query {query}", ex);
            }

            return bills;
            */
        }

        private string AppendPageAndOffsetToQuery(string query, int page, int resultsPerPage)
        {
            //if (string.IsNullOrWhiteSpace(query))
            //    throw new ArgumentException($"Error: Cannot retrieve Votes for legislator ({legislatorBioguideId}) because of invalid query: {query}");

            if (page < 0)
                page = 0;
            //throw new ArgumentException($"Error: Cannot retrieve Votes for legislator ({legislatorBioguideId}) because of invalid page value: {page}");


            //if (resultsPerPage < 1)
            //    resultsPerPage = defaultResultsPage;
			
            var resultsOffSet = page * resultsPerPage;

            var uri = $"{query}?offset={resultsOffSet}";

            /*
            var uri = string.Format("{0}{1}{2}{3}{4}{5}",
                _votesBase,
                query,
                _fields,
                _filedAppedix + legislatorBioguideId,
                _perPage + resultsPerPage,
                _page + page.ToString());
            */

            return uri;
        }
    }
}
