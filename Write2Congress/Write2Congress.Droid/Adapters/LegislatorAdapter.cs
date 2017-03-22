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

        public LegislatorAdapter(BaseFragment fragment, List<Legislator> legislators) 
        {
            Logger = new Logger(Class.SimpleName);
            _legislators = legislators;
            _fragment = fragment;

            _termStartDate = AndroidHelper.GetString(Resource.String.termStarted);
            _termEndDate = AndroidHelper.GetString(Resource.String.termEnds);
            _senate = AndroidHelper.GetString(Resource.String.senate);
            _congress = AndroidHelper.GetString(Resource.String.congress);

            //TODO RM: Ensure this works with pre 5.0 like 4.4
            try
            {
                _fragment.Activity.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, _selectableItemBackground, true);
            }
            catch (Exception e)
            {
                Logger.Error($"An Error occurred while retrieving the SelectableItemBackground used for transparent buttons. {e.Message}");
                _selectableItemBackground = null;
            }
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
                ContactMethodAction(contactMethod, false);
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

            return new LegislatorAdapterViewHolder(legislatorView, OnWriteLetterToLegislatorClick, OnActionButtonClick);
        }

        /// <summary>
        /// Replace the contents of a view (invoked by the layout manager)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //TODO RM: Since position is not zero base, do we need to decrease 1 from it?
            var legislator = _legislators[position];
            var viewHolder = holder as LegislatorAdapterViewHolder;

            //Portrait
            SetLegislatorPortrait(legislator, viewHolder.Portrait);

            //Basic Info
            viewHolder.Chamber.Text = $"{legislator.Chamber} ({legislator.State.ToString()})";
            viewHolder.Name.Text = legislator.FullName;
            viewHolder.TermStartDate.Text = legislator.TermStartDate.Equals(DateTime.MinValue)
                ? $"{_termStartDate}: {AndroidHelper.GetString(Resource.String.unknown)}"
                : $"{_termStartDate}: {legislator.TermStartDate.ToShortDateString()}";
            viewHolder.TermEndDate.Text = legislator.TermEndDate.Equals(DateTime.MinValue)
                ? $"{_termEndDate}: {AndroidHelper.GetString(Resource.String.unknown)}"
                : $"{_termEndDate}: {legislator.TermEndDate.ToShortDateString()}";

            //Contact, social media, ect buttons
            SetImageButtonVisibility(viewHolder.WriteLetter, legislator.Email);
            SetImageButtonVisibility(viewHolder.Email, legislator.Email);
            SetImageButtonVisibility(viewHolder.Phone, legislator.OfficeNumber);
            SetImageButtonVisibility(viewHolder.Address, legislator.OfficeAddress);

            SetImageButtonVisibility(viewHolder.Facebook, legislator.FacebookId);
            SetImageButtonVisibility(viewHolder.Twitter, legislator.TwitterId);
            SetImageButtonVisibility(viewHolder.Webpage, legislator.Website);
            SetImageButtonVisibility(viewHolder.YouTube, legislator.YouTubeId);
        }

        private void SetLegislatorPortrait(Legislator legislator, ImageView imageButton)
        {           
            switch (legislator.Party)
            {
                case Shared.DomainModel.Enum.Party.Democratic:
                    imageButton.SetImageResource(Resource.Drawable.ic_democratic_logo);
                    break;
                case Shared.DomainModel.Enum.Party.Republican:
                    imageButton.SetImageResource(Resource.Drawable.ic_republican_elephant);
                    break;
                case Shared.DomainModel.Enum.Party.Independent:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Shared.DomainModel.Enum.Party.Libertarian:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Shared.DomainModel.Enum.Party.Green:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Shared.DomainModel.Enum.Party.Unknown:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                default:
                    break;
            }
        }

        private void SetImageButtonVisibility(ImageView imageButton, ContactMethod contactMethod)
        {
            imageButton.Visibility = contactMethod.IsEmpty
                ? ViewStates.Gone
                : ViewStates.Visible;

            if (_selectableItemBackground != null)
                imageButton.SetBackgroundResource(_selectableItemBackground.ResourceId);
        }

        protected void ContactMethodAction(ContactMethod contactMethod, bool useChooser)
        {
            var intent = useChooser
                ? Intent.CreateChooser(AppHelper.GetIntentForContactMethod(contactMethod), "Open with")
                : new Intent(AppHelper.GetIntentForContactMethod(contactMethod));

            _fragment.StartActivity(intent);
        }
    }
}