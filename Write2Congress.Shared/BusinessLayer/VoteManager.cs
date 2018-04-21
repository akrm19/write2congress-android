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
        private const int _defaultResultsPerPage = 20;

        public VoteManager(IMyLogger logger)
        {
            _voteSvc = new VoteSvc(logger);
        }

        public ApiResultWithMoreResultIndicator<Vote> GetLegislatorVotes(string legislatorBioguideId, int page, int resultsPerPage = _defaultResultsPerPage)
        {
            var votes = new List<Vote>();

            var votesResult = _voteSvc.GetVotesByLegislator(legislatorBioguideId, page, resultsPerPage);

            foreach (var iVote in votesResult.Results)
                votes.Add(Vote.TransformToVote(iVote));


            var results = new ApiResultWithMoreResultIndicator<Vote>(votes, votesResult.IsThereMoreResults);

            return results;
        }

        #region Vote Helper Methods
        /// <summary>
        /// Custom method to get the text to show when clicking on a Vote
        /// </summary>
        /// <returns></returns>
        public static string GetVoteSummary(Vote vote)
        {
			var text = new StringBuilder();

                
            var questionLabel = "Question";
            if (!string.IsNullOrWhiteSpace(vote.Question))
                text.AppendLine($"{questionLabel}: {vote.Question}").AppendLine();

            var descriptionLabel = "Description";
            if (!string.IsNullOrWhiteSpace(vote.Description))
                text.AppendLine($"{descriptionLabel}: {vote.Description}").AppendLine();

            var voteCasted = "Vote Casted";
            text.AppendLine($"{voteCasted}: {vote.VoteCastedByLegislator.GetDescription()}").AppendLine();
   
            var result = "Vote Result";
            var resultText = string.IsNullOrWhiteSpace(vote.Result)
                                   ? "Unknown"
                                   : vote.Result;
            text.AppendLine($"{result}: {resultText}").AppendLine();


            text.AppendLine(GetVoteResultBreakdown(vote));

			var dateLabel = "Date";
			if (vote.VotedAt != DateTime.MinValue)
				text.AppendLine($"{dateLabel}: {vote.VotedAt.ToString("d")}").AppendLine();
			
            if (!string.IsNullOrWhiteSpace(GetVoteMoreInfoTitle(vote)))
                text.AppendLine(GetVoteMoreInfoTitle(vote)).AppendLine();

            return text.ToString();
        }

        public static string GetVoteResultBreakdown(Vote vote)
        {
            if (vote == null)
                return string.Empty;

            var voteBreakdown = new StringBuilder("Vote Breakdown").AppendLine();

            if(vote.VoteResults.Yes != -1)
                voteBreakdown.AppendLine($"Yes: {vote.VoteResults.Yes.ToString()}");

            if (vote.VoteResults.No != -1)
                voteBreakdown.AppendLine($"No: {vote.VoteResults.No.ToString()}");

            if(vote.VoteResults.Present != -1)
                voteBreakdown.AppendLine($"Present: {vote.VoteResults.Present.ToString()}");

            if (vote.VoteResults.NotVoting != -1)
                voteBreakdown.AppendLine($"Not Voting: {vote.VoteResults.NotVoting.ToString()}");

            return voteBreakdown.AppendLine().ToString();
        }

        public static string GetVoteDisplayTitle(Vote vote)
        {
            var title = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(vote.Question))
                title.Append($"{vote.Question}");

            if (!string.IsNullOrWhiteSpace(vote.Description))
            {
                title.AppendFormat("{0}{1}",
                    string.IsNullOrWhiteSpace(vote.Question)
                        ? string.Empty
                        : ": ",
                    vote.Description);
            }

            return title.ToString();
        }

        public static string GetVoteMoreInfoTitle(Vote vote)
        {
            if (vote.Bill != null && !string.IsNullOrWhiteSpace(vote.Bill.GetDisplayTitle()))
                return vote.Bill.GetDisplayTitleWithLabel();

            else if (vote.Nomination != null && !string.IsNullOrWhiteSpace(vote.Nomination.GetDisplayTitle()))
                return vote.Nomination.GetDisplayTitle();

            return string.Empty;
        }
        #endregion
    }
}
