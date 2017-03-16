using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Write2Congress.Droid.Adapters
{
    public class LetterAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image;

        public TextView Subject;
        public TextView Recipient;
        public TextView LastSavedDate;
        public TextView SendDate;

        public ImageButton OpenOrEdit;
        public ImageButton Copy;
        public ImageButton Delete;

        public LetterAdapterViewHolder(View itemView) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.letterCtrl_image);

            Subject = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_subject);
            Recipient = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_recipient);
            LastSavedDate = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_lastSavedDate);
            SendDate = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_sentDate);

            OpenOrEdit = itemView.FindViewById<ImageButton>(Resource.Id.letterCrl_viewOrEdit);
            Copy = itemView.FindViewById<ImageButton>(Resource.Id.letterCtrl_copy);
            Delete = itemView.FindViewById<ImageButton>(Resource.Id.letterCtrl_delete);
        }
    }
}