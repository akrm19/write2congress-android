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
using Write2Congress.Droid.CustomControls;
using Write2Congress.Shared.BusinessLayer;
using System.Threading.Tasks;

namespace Write2Congress.Droid.Fragments
{
    public class ViewLegislatorFragment : BaseFragment
    {
        private Legislator _legislator;
        private CommitteeViewer _committeeViewer;
        private BillViewer _SponsoredBillsViewer;
        private List<Bill> _sponsoredBills;
        private BillManager _billManager;

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

            _billManager = new BillManager(MyLogger);

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

            PopulateBasicInfo(fragment, _legislator);
            PopulateContactMethodsButtons(fragment, _legislator);
            PopulateCommitteeViewer(fragment, _legislator);
            PopulateSponsoredBills(fragment, _legislator);

            return fragment;
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

        private void PopulateContactMethodsButtons(View fragment, Legislator legislator)
        {
            var WriteLetter = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_writeLetter);
            var Email = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_email);
            var Phone = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_phone);
            var Address = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_address);

            var Facebook = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_facebook);
            var Twitter = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_twitter);
            var Webpage = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_webpage);
            var YouTube = fragment.FindViewById<Button>(Resource.Id.viewLegislatorFrag_youtube);

            //Contact, social media, ect buttons
            SetupLegislatorContactMthdButton(WriteLetter, legislator.Email);
            SetupLegislatorContactMthdButton(Email, legislator.Email);
            SetupLegislatorContactMthdButton(Phone, legislator.OfficeNumber);
            SetupLegislatorContactMthdButton(Address, legislator.OfficeAddress);
            
            SetupLegislatorContactMthdButton(Facebook, legislator.FacebookId);
            SetupLegislatorContactMthdButton(Twitter, legislator.TwitterId);
            SetupLegislatorContactMthdButton(Webpage, legislator.Website);
            SetupLegislatorContactMthdButton(YouTube, legislator.YouTubeId);
        }

        public void SetupLegislatorContactMthdButton(View button, ContactMethod contactMethod)
        {
            button.Visibility = contactMethod.IsEmpty
                ? ViewStates.Gone
                : ViewStates.Visible;

            button.Click += (sender, e) => ContactButton_Click(contactMethod);
        }

        private void ContactButton_Click(ContactMethod contactMethod)
        {
            AppHelper.PerformContactMethodIntent(this as BaseFragment, contactMethod, false);
        }

        private void PopulateCommitteeViewer(View fragmentView, Legislator _legislator)
        {
            _committeeViewer = fragmentView.FindViewById<CommitteeViewer>(Resource.Id.viewLegislatorFrag_committeViewer);
            _committeeViewer.SetupCtrl(this);
            _committeeViewer.ShowLegislatorCommittees(_legislator);
        }

        private void PopulateSponsoredBills(View fragment, Legislator _legislator)
        {
            //if (_SponsoredBillsViewer == null)
            //{
                _SponsoredBillsViewer = fragment.FindViewById<BillViewer>(Resource.Id.viewLegislatorFrag_BillViewer);
                _SponsoredBillsViewer.SetupCtrl(this);
            //}

            if (_sponsoredBills == null)
            {
                var getBillsTask = new Task<List<Bill>>(
                    () => _billManager.GetBillsSponsoredbyLegislator(_legislator.BioguideId, 1)
                    //_SponsoredBillsViewer.UpdateBills(_sponsoredBills);
                );

                getBillsTask.ContinueWith((antecedent) =>
                {
                    _sponsoredBills = antecedent.Result;
                    Activity.RunOnUiThread(() =>
                    {
                        _SponsoredBillsViewer.UpdateBills(_sponsoredBills);
                    });
                });
                
                getBillsTask.Start();
            }
            else
                _SponsoredBillsViewer.UpdateBills(_sponsoredBills);
        }
    }
}