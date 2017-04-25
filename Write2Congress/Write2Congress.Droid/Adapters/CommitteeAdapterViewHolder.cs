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
    public class CommitteeAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image;
        public TextView Name;
        public Button Phone;
        public Button Website;

        public CommitteeAdapterViewHolder(View view, Action<int, int> actionButtonListner) : base(view)
        {
            Image = view.FindViewById<ImageView>(Resource.Id.committeeCtrl_image);
            Image.SetBackgroundResource(Resource.Color.accent_purple);
            Image.SetImageResource(Resource.Drawable.ic_group_white_48dp);

            Name = view.FindViewById<TextView>(Resource.Id.committeeCtrl_name);

            Phone = view.FindViewById<Button>(Resource.Id.committeeCtrl_phone);
            Phone.Click += (sender, e) => actionButtonListner(AdapterPosition, (sender as View).Id);

            Website = view.FindViewById<Button>(Resource.Id.committeeCtrl_webpage);
            Website.Click += (sender, e) => actionButtonListner(AdapterPosition, (sender as View).Id);
        }
    }
}