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

        public ImageButton Copy;
        public ImageButton Delete;

        public LetterAdapterViewHolder(View itemView, Action<int> onClickListener, Action<int> onCopyClickListner, Action<int> onDeleteClickListener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.letterCtrl_image);

            Subject = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_subject);
            Recipient = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_recipient);
            LastSavedDate = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_lastSavedDate);
            SendDate = itemView.FindViewById<TextView>(Resource.Id.letterCtrl_sentDate);

            Copy = itemView.FindViewById<ImageButton>(Resource.Id.letterCtrl_copy);
            Copy.Click -= (sender, e) => onCopyClickListner(AdapterPosition);
            Copy.Click += (sender, e) => onCopyClickListner(AdapterPosition);

            Delete = itemView.FindViewById<ImageButton>(Resource.Id.letterCtrl_delete);
            Delete.Click -= (sender, e) => onDeleteClickListener(AdapterPosition);
            Delete.Click += (sender, e) => onDeleteClickListener(AdapterPosition);

            itemView.Click -= (sender, e) => onClickListener(AdapterPosition);
            itemView.Click += (sender, e) => onClickListener(AdapterPosition);
        }
    }
}