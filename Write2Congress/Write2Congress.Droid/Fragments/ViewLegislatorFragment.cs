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
using Write2Congress.Shared.DomainModel;
using Fragment = Android.Support.V4.App.Fragment;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Newtonsoft.Json;

namespace Write2Congress.Droid.Fragments
{
    public class ViewLegislatorFragment : BaseFragment
    {
        private Legislator _legislator;
        private TypedValue _selectableItemBackground;

        //Note: Fragment sub-classes must have a public default no argument constructor.
        //TODO RM: FIXX!!!
        //Empty construct needed due to:
        //http://stackoverflow.com/questions/33309926/android-app-crash-after-taking-picture-unable-to-find-the-default-constructor
        public ViewLegislatorFragment()
        {}
        //http://stackoverflow.com/questions/19320008/default-constructors-in-xamarin-android


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //https://developer.xamarin.com/guides/android/platform_features/fragments/part_1_-_creating_a_fragment/
            //SetRetainInstance(true)

            var serializedLegislator = Arguments.GetString(BundleType.Legislator);

            if (string.IsNullOrWhiteSpace(serializedLegislator))
            {
                MyLogger.Error("No legislator passed with creating ViewLegislatorFragment. Returning");
                Activity.Finish();
                return;
            }

            _legislator = JsonConvert.DeserializeObject<Legislator>(serializedLegislator);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;

            var fragment = inflater.Inflate(Resource.Layout.frag_ViewLegislator, container, false);


            //TODO RM: This might be null
            _selectableItemBackground = AppHelper.GetTypedValueFromActv(this.Activity);

            PopulateBasicInfo(fragment, _legislator);
            PopulateContactMethodsButtons(fragment, _legislator, _selectableItemBackground);

            return fragment;
        }

        private void PopulateContactMethodsButtons(View fragment, Legislator legislator, TypedValue selectableItemBackground)
        {
            var WriteLetter = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_writeLetter);
            var Email = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_email);
            var Phone = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_phone);
            var Address = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_address);

            var Facebook = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_facebook);
            var Twitter = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_twitter);
            var Webpage = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_webpage);
            var YouTube = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_youtube);

            //Contact, social media, ect buttons
            AppHelper.SetLegislatorContactMthdVisibility(WriteLetter, legislator.Email, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(Email, legislator.Email, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(Phone, legislator.OfficeNumber, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(Address, legislator.OfficeAddress, selectableItemBackground);

            //AppHelper.SetLegislatorContactMthdVisibility(Facebook, legislator.FacebookId, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(Twitter, legislator.TwitterId, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(Webpage, legislator.Website, selectableItemBackground);
            AppHelper.SetLegislatorContactMthdVisibility(YouTube, legislator.YouTubeId, selectableItemBackground);

            
        }
        
        private void PopulateBasicInfo(View fragment, Legislator legislator)
        {
            using (var portrait = fragment.FindViewById<ImageView>(Resource.Id.viewLegislatorFrag_portrait))
            {
                AppHelper.SetLegislatorPortrait(legislator, portrait);
            }

            using (var chamber = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_chamber))
                chamber.Text = $"{legislator.Chamber} ({legislator.State.ToString()})";

            using (var name = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_name))
                name.Text = legislator.FullName;

            var termStartDateText = AndroidHelper.GetString(Resource.String.termStarted);
            using (var termStartDate = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_termStartDate))
                termStartDate.Text = AppHelper.GetLegislatorTermStartDate(legislator, termStartDateText);

            var termEndDateText = AndroidHelper.GetString(Resource.String.termEnds);
            using (var termEndDate = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_termEndDate))
                termEndDate.Text = AppHelper.GetLegislatorTermEndDate(legislator, termEndDateText);
        }
    }
}