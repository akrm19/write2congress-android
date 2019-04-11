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
    public class LegislatorAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Portrait { get; private set; }
        public TextView Chamber { get; private set; }
        public TextView Name { get; private set; }
        public TextView TermStartDate { get; private set; }
        public TextView TermEndDate { get; private set; }

        public ImageButton Email { get; private set; }
        public ImageButton Phone { get; private set; }
        public ImageButton Address { get; private set; }

        public ImageButton Facebook { get; private set; }
        public ImageButton Twitter { get; private set; }
        public ImageButton YouTube { get; private set; }
        public ImageButton Webpage { get; private set; }

        public LegislatorAdapterViewHolder(View itemView, Action<int, int> actionButtonListner, Action<int> legislatorClickListner) :base(itemView)
        {
            itemView.Click += (sender, e) => legislatorClickListner(base.AdapterPosition);

            Portrait = itemView.FindViewById<ImageView>(Resource.Id.legislatorCtrl_portrait);
            Chamber = itemView.FindViewById<TextView>(Resource.Id.legislatorCtrl_chamber);
            Name = itemView.FindViewById<TextView>(Resource.Id.legislatorCtrl_name);
            TermStartDate = itemView.FindViewById<TextView>(Resource.Id.legislatorCtrl_termStartDate);
            TermEndDate = itemView.FindViewById<TextView>(Resource.Id.legislatorCtrl_termEndDate);

            Email = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_email);
            Email.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            Phone = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_phone);
            Phone.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            Address = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_address);
            Address.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            Facebook = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_facebook);
            Facebook.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            Twitter = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_twitter);
            Twitter.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            Webpage = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_webpage);
            Webpage.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);

            YouTube = itemView.FindViewById<ImageButton>(Resource.Id.legislatorCtrl_youtube);
            YouTube.Click += (sender, e) => actionButtonListner(base.AdapterPosition, (sender as View).Id);
        }
    }
}