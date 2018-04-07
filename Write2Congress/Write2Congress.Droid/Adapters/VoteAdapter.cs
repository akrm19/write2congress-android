using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.DomainModel;
using Android.Support.V7.Widget;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid.Adapters
{
    public class VoteAdapter : RecyclerView.Adapter
    {
        private string voteResult, voteType, date, billInfo, nominationInfo;

        private BaseFragment _fragment;
        private Logger _logger;
        private List<Vote> _votes = new List<Vote>();

        public VoteAdapter(BaseFragment fragment)
        {
            _fragment = fragment;
            _logger = new Logger(Class.SimpleName);

            voteResult = AndroidHelper.GetString(Resource.String.voteResult);
            voteType = AndroidHelper.GetString(Resource.String.voteType);
            date = AndroidHelper.GetString(Resource.String.date);
            billInfo = AndroidHelper.GetString(Resource.String.billInfo);
            nominationInfo = AndroidHelper.GetString(Resource.String.nominationInfo);
        }

        public override int ItemCount
        {
            get
            {
                return _votes.Count;
            }
        }

        public void UpdateVotes(List<Vote> votes)
        {
            _votes = votes;
            NotifyDataSetChanged();
        }

        private void OnVoteClick(int position)
        {
            var vote = _votes[position];

            if (vote == null)
            {
                _logger.Error("Cannot process Vote click event. Unable to find vote at position " + position);
                return;
            }

            var title = vote.Type?.Type.GetDescription();
            var summary = VoteManager.GetVoteSummary(vote);
            AppHelper.ShowDetailsDialog(_fragment, title, summary, vote.Source);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var voteView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_Vote, parent, false);
            return new VoteAdapterViewHolder(voteView, OnVoteClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vote = _votes[position] ?? null;
            if (vote == null)
            {
                _logger.Error("Cannot bind vote. Unable to find vote at position " + position);
                return;
            }

            var viewHolder = holder as VoteAdapterViewHolder;
            viewHolder.Question.Text = vote.Question;

            if (string.IsNullOrWhiteSpace(vote.Result))
                viewHolder.VoteResult.Visibility = ViewStates.Gone;
            else
            {
                viewHolder.VoteResult.Visibility = ViewStates.Visible;
                viewHolder.VoteResult.Text = $"{voteResult}: {vote.Result}";
            }


            if (string.IsNullOrWhiteSpace(vote?.Type?.Value))
                viewHolder.VoteType.Visibility = ViewStates.Gone;
            else
            {
                viewHolder.VoteType.Visibility = ViewStates.Visible;
				viewHolder.VoteType.Text = $"{voteType}: {vote.Type.Value.Capitalize()}"; 
            }

            if (vote?.VotedAt == DateTime.MinValue)
                viewHolder.VotedAt.Visibility = ViewStates.Gone;
            else
            {
                viewHolder.VotedAt.Visibility = ViewStates.Visible;
				viewHolder.VotedAt.Text = $"{date}: {vote.VotedAt.ToShortDateString()}";
            }

            var voteMoreInfo = VoteManager.GetVoteMoreInfoTitle(vote);

            viewHolder.MoreInfo.Text = voteMoreInfo;
            viewHolder.MoreInfo.Visibility = string.IsNullOrWhiteSpace(voteMoreInfo)
                ? ViewStates.Gone
                : ViewStates.Visible;

            viewHolder.Image.SetImageResource(GetImageResourceForVoteCastedType(vote.VoteCastedByLegislator));
        }

        private int GetImageResourceForVoteCastedType(VoteCastedType voteCasted)
        {
            switch (voteCasted)
            {
                case VoteCastedType.Nay:
                    return Resource.Drawable.ic_thumb_down_white_48dp;
                case VoteCastedType.Yea:
                    return Resource.Drawable.ic_thumb_up_white_48dp;
                case VoteCastedType.NotVoting:
                    return Resource.Drawable.ic_do_not_disturb_alt_white_48dp;
                case VoteCastedType.Present:
                    return Resource.Drawable.ic_pan_tool_white_48dp;
                case VoteCastedType.Unknown:
                default:
                    return Resource.Drawable.ic_insert_drive_file_white_48dp;
            }
        }
    }
}