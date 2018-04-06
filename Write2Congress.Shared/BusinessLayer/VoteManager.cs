﻿using System;
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

        public bool IsThereMoreResultsForLastCall()
        {
            //TODO R<: Fix
            return true;//_voteSvc.IsThereMoreResults();
        }

        public List<Vote> GetLegislatorVotes(string legislatorBioguideId, int page, int resultsPerPage = _defaultResultsPerPage)
        {
            var votes = new List<Vote>();

            var votesResult = _voteSvc.GetVotesByLegislator(legislatorBioguideId, page, resultsPerPage);

            foreach (var iVote in votesResult)
                votes.Add(Vote.TransformToVote(iVote));


            return votes;
        }

        /// <summary>
        /// Custom method to get the text to show when clicking on a Vote
        /// </summary>
        /// <returns></returns>
        public static string GetVoteSummary(Vote vote)
        {
            var voteCasted = "Vote Casted";
            var question = "Question";
            var result = "Vote Result";
            var type = "Vote Type";
            var date = "Date";
            
            var text = new StringBuilder();
            text.AppendLine($"{question}: {vote.Question}")
            .AppendLine()
            .AppendLine($"{voteCasted}: {vote.VoteCastedByLegislator.GetDescription()}")
            .AppendLine()
            .AppendLine($"{result}: {vote.Result}")
            .AppendLine()
            .AppendLine($"{type}: {vote.Type.Value.Capitalize()}")
            .AppendLine()
            .AppendLine($"{date}: {vote.VotedAt.ToString("d")}")
            .AppendLine();

            if (!string.IsNullOrWhiteSpace(GetVoteMoreInfoTitle(vote)))
                text.AppendLine(GetVoteMoreInfoTitle(vote)).AppendLine();

            return text.ToString();
        }

        public static string GetVoteMoreInfoTitle(Vote vote)
        {
            if (vote.Bill != null)
                return vote.Bill.GetDisplayTitleWithLabel();

            else if (vote.Nomination != null)
                return vote.Nomination.GetDisplayTitle();

            return string.Empty;
        }
    }
}
