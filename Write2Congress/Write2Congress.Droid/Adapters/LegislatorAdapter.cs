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

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorAdapter : RecyclerView.Adapter
    {
        protected Logger Logger;
        private List<Legislator> _legislators;
        private BaseFragment _fragment;
        private TypedValue _selectableItemBackground = new TypedValue();
        private string _termStartDate, _termEndDate, _senate, _congress;

        //public LegislatorAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        //{ }

        public LegislatorAdapter(BaseFragment fragment, List<Legislator> legislators) //: base()
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

        public void UpdateLegislators(List<Legislator> legislators)
        {
            _legislators = legislators;
            NotifyDataSetChanged();
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
            SetImageButton(viewHolder.Email, legislator.Email);
            SetImageButton(viewHolder.Phone, legislator.OfficeNumber);
            SetImageButton(viewHolder.Address, legislator.OfficeAddress);

            SetImageButton(viewHolder.Facebook, legislator.FacebookId);
            SetImageButton(viewHolder.Twitter, legislator.TwitterId);
            SetImageButton(viewHolder.Webpage, legislator.Website);
            SetImageButton(viewHolder.YouTube, legislator.YouTubeId);
        }

        private void SetLegislatorPortrait(Legislator legislator, ImageView imageButton)
        {
            var portraitBitmap = AppHelper.GetPortraitForLegislator(legislator);

            if (portraitBitmap != null)
                imageButton.SetImageBitmap(portraitBitmap);
            else
                SetDefaultLegislatorPortrait(legislator, imageButton);
        }

        private void SetDefaultLegislatorPortrait(Legislator legislator, ImageView imageButton)
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

        private void SetImageButton(ImageView imageButton, ContactMethod contactMethod)
        {
            imageButton.Visibility = contactMethod.IsEmpty
                ? ViewStates.Gone
                : ViewStates.Visible;

            if(_selectableItemBackground != null)
                imageButton.SetBackgroundResource(_selectableItemBackground.ResourceId);

            //TODO RM: Is unsubscribe method needed or should Item click should be implemented differently?
            //Example: https://github.com/xamarin/monodroid-samples/blob/master/android5.0/RecyclerViewer/RecyclerViewer/MainActivity.cs
            imageButton.Click -= (sender, e) => ContactMethodAction(contactMethod);
            imageButton.Click += (sender, e) => ContactMethodAction(contactMethod);
        }

        protected void ContactMethodAction(ContactMethod contactMethod)
        {
            var intent = Intent.CreateChooser(AppHelper.GetIntentForContactMethod(contactMethod), "Open with");
            _fragment.StartActivity(intent);
        }
    }
}