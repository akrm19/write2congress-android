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
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Activities;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorAdapter : RecyclerView.Adapter
    {
        private List<Legislator> _legislators;
        private BaseActivity _activity;
        private BaseFragment _fragment;
        
        public LegislatorAdapter(BaseFragment fragment, List<Legislator> legislators)
        {
            _legislators = legislators;
            _fragment = fragment;
        }

        public LegislatorAdapter(BaseActivity activity, List<Legislator> legislators)
        {
            _legislators = legislators;
            _activity = activity;
        }

        public override int ItemCount
        {
            get
            {
                return _legislators.Count;
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var legislatorView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_legislator, parent, false);

            return new LegislatorAdapterViewHolder(legislatorView);
        }

        // Replace the contents of a view (invoked by the layout manager
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var legislator = _legislators[position];

            var viewHolder = holder as LegislatorAdapterViewHolder;

            //Portrait
            viewHolder.Portrait.SetImageResource(Resource.Drawable.ic_person_black_48dp);// = //legislator.p //TODO RM: Add portrait

            //Basic Info
            viewHolder.Chamber.Text = legislator.Chamber.ToString() ?? string.Empty; //TODO RM Localize
            viewHolder.Name.Text = legislator.FullName;
            viewHolder.TermStartDate.Text = legislator.TermStartDate.ToShortDateString() ?? string.Empty;
            viewHolder.TermEndDate.Text = legislator.TermEndDate.ToShortDateString() ?? string.Empty;

            //Contact, social media, ect buttons
            SetImageButton(viewHolder.Email, legislator.Email);
            SetImageButton(viewHolder.Facebook, legislator.FacebookId);
            SetImageButton(viewHolder.Twitter, legislator.TwitterId);
            SetImageButton(viewHolder.Webpage, legislator.Website);
            SetImageButton(viewHolder.Phone, legislator.OfficeNumber);
            SetImageButton(viewHolder.Address, legislator.OfficeAddress);
        }

        private void SetImageButton(ImageButton imageButton, ContactMethod legislatorcontact)
        {
            if (legislatorcontact.IsEmpty)
            {
                imageButton.Visibility = ViewStates.Gone;
            }
            else
            {
                imageButton.Visibility = ViewStates.Visible;
                //Todo, set onlick & unbind onclick in not available
            }
        }



    }
}