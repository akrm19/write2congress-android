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
    public class BillAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image;
        public TextView Name;
        public TextView DateIntroduced;
        public TextView Cosponsors;
        public TextView Status;
        public TextView StatusDate;
        public TextView LastActionDate;
        public TextView LastActionText;
        public TextView Summary;

        public BillAdapterViewHolder(View view, Action<int> billClickListner) : base(view)
        {
            view.Click += (sender, e) => billClickListner(base.AdapterPosition);

            Image = view.FindViewById<ImageView>(Resource.Id.billCtrl_image);
            Image.SetBackgroundResource(Resource.Color.accent_purple);
            Image.SetImageResource(Resource.Drawable.ic_insert_drive_file_white_48dp);

            Name = view.FindViewById<TextView>(Resource.Id.billCtrl_name);
            DateIntroduced = view.FindViewById<TextView>(Resource.Id.billCtrl_dateIntroduced);
            Cosponsors = view.FindViewById<TextView>(Resource.Id.billCtrl_cosponsors);
            Status = view.FindViewById<TextView>(Resource.Id.billCtrl_status);
            StatusDate = view.FindViewById<TextView>(Resource.Id.billCtrl_statusDate);
            LastActionDate = view.FindViewById<TextView>(Resource.Id.billCtrl_lastActionDate);
            LastActionText = view.FindViewById<TextView>(Resource.Id.billCtrl_lastActionText);
            Summary = view.FindViewById<TextView>(Resource.Id.billCtrl_summary);
        }
    }
}