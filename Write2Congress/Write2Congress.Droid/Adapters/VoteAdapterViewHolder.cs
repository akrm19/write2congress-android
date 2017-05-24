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
using Android.Support.V7.Widget;

namespace Write2Congress.Droid.Adapters
{
    public class VoteAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image;
        public TextView Question;
        public TextView VoteResult;
        public TextView VoteType;
        public TextView VotedAt;
        public TextView MoreInfo;

        public VoteAdapterViewHolder(View view, Action<int> voteClickListner) : base(view)
        {
            view.Click += (sender, e) => voteClickListner(base.AdapterPosition);

            Image = view.FindViewById<ImageView>(Resource.Id.voteCtrl_image);
            Image.SetBackgroundResource(Resource.Color.primary_blue_dark);
            Image.SetImageResource(Resource.Drawable.ic_insert_drive_file_white_48dp);

            Question = view.FindViewById<TextView>(Resource.Id.voteCtrl_question);
            VoteResult = view.FindViewById<TextView>(Resource.Id.voteCtrl_voteResult);
            VoteType = view.FindViewById<TextView>(Resource.Id.voteCtrl_voteType);
            VotedAt = view.FindViewById<TextView>(Resource.Id.voteCtrl_votedAt);
            MoreInfo = view.FindViewById<TextView>(Resource.Id.voteCtrl_moreInfo);
        }
    }
}