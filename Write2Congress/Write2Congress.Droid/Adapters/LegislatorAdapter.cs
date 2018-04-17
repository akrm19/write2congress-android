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
using Write2Congress.Droid.Code;
using Android.Util;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorAdapter : RecyclerView.Adapter
    {
        protected Logger Logger;
        private List<Legislator> _legislators;
        private BaseFragment _fragment;
        private TypedValue _selectableItemBackground = new TypedValue();
        private string _termStartDate, _termEndDate, _senate, _congress;

        public event EventHandler<int> WriteLetterToLegislatorClick;
        public event EventHandler<int> LegislatorClick;

        public LegislatorAdapter(BaseFragment fragment, List<Legislator> legislators) 
        {
            Logger = new Logger(Class.SimpleName);
            _legislators = legislators;
            _fragment = fragment;

            _termStartDate = AndroidHelper.GetString(Resource.String.termStarted);
            _termEndDate = AndroidHelper.GetString(Resource.String.termEnds);
            _senate = AndroidHelper.GetString(Resource.String.senate);
            _congress = AndroidHelper.GetString(Resource.String.congress);

            _selectableItemBackground = AppHelper.GetTypedValueFromActv(_fragment.Activity);
        }

        public override int ItemCount
        {
            get
            {
                return _legislators.Count;
            }
        }

        public Legislator GetLegislatorAtPosition(int position)
        {
            
            if (_legislators.Count > position)
                return _legislators[position];

            return null;
        }

        public void UpdateLegislators(List<Legislator> legislators)
        {
            _legislators = legislators;
            NotifyDataSetChanged();
        }
        private void OnLegislatorClick(int position)
        {
            LegislatorClick?.Invoke(this, position);
        }

        private void OnWriteLetterToLegislatorClick(int position)
        {
            WriteLetterToLegislatorClick?.Invoke(this, position);
        }

        private void OnActionButtonClick(int position, int buttonResourceId)
        {
            var legislator = GetLegislatorAtPosition(position);

            if(legislator == null)
            {
                Logger.Error($"Unable to process legislator's action button click. Unable to find legislator at position {position}");
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.unableToProcessAction));
                return;
            }

            ContactMethod contactMethod = null;

            switch (buttonResourceId)
            {
                case Resource.Id.legislatorCtrl_email:
                    contactMethod = legislator.Email;
                    break;
                case Resource.Id.legislatorCtrl_phone:
                    contactMethod = legislator.OfficeNumber;
                    break;
                case Resource.Id.legislatorCtrl_address:
                    contactMethod = legislator.OfficeAddress;
                    break;
                case Resource.Id.legislatorCtrl_facebook:
                    contactMethod = legislator.FacebookId;
                    break;
                case Resource.Id.legislatorCtrl_twitter:
                    contactMethod = legislator.TwitterId;
                    break;
                case Resource.Id.legislatorCtrl_youtube:
                    contactMethod = legislator.YouTubeId;
                    break;
                case Resource.Id.legislatorCtrl_webpage:
                    contactMethod = legislator.Website;
                    break;
            }

            if (contactMethod != null)
                AppHelper.PerformContactMethodIntent(_fragment, contactMethod, false);
        }

        /// <summary>
        /// Create new views (invoked by the layout manager)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var legislatorView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_Legislator, parent, false);

            return new LegislatorAdapterViewHolder(legislatorView, OnWriteLetterToLegislatorClick, OnActionButtonClick, OnLegislatorClick);
        }

        /// <summary>
        /// Replace the contents of a view (invoked by the layout manager)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var legislator = _legislators[position];
            var viewHolder = holder as LegislatorAdapterViewHolder;

            //Portrait
            AppHelper.SetLegislatorPortrait(legislator, viewHolder.Portrait);

            //Basic Info
            viewHolder.Chamber.Text = $"{legislator.Chamber} ({legislator.State.ToString()})";
            viewHolder.Name.Text = legislator.FullName();
            viewHolder.TermStartDate.Text = AppHelper.GetLegislatorTermStartDate(legislator, _termStartDate);
            viewHolder.TermEndDate.Text = AppHelper.GetLegislatorTermEndDate(legislator, _termEndDate);

            //Contact, social media, ect buttons
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.WriteLetter, legislator.Email, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Email, legislator.Email, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Phone, legislator.OfficeNumber, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Address, legislator.OfficeAddress, _selectableItemBackground);
            
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Facebook, legislator.FacebookId, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Twitter, legislator.TwitterId, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.Webpage, legislator.Website, _selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(viewHolder.YouTube, legislator.YouTubeId, _selectableItemBackground);
        }
    }
}