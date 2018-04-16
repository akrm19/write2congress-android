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
using Write2Congress.Shared.BusinessLayer;
using System.Threading.Tasks;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Write2Congress.Droid.CustomControls;
using Write2Congress.Droid.Adapters;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Android.Support.V7.Widget;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Fragments
{
    public class ViewLegislatorFragment : BaseFragment
    {
        private Legislator _legislator;
        private LegislatorViewPagerAdapter _viewPagerAdapter;
        //private Shared.BusinessLayer.Services.LegislatorSvc ls;
        private LegislatorManager _legistorManager;
        private TypedValue _selectableItemBackground;
        private ImageView _portrait;
        Android.Graphics.Bitmap portraitAsBitmap;

        //vote
        private int _votesCurrentPage = 1;
        private bool _votesIsThereMoreContent = false;
        private List<Vote> _votes;
        private VoteManager _voteManager;
        protected RecyclerView.Adapter _voteRecyclerAdapter;

        private BillManager _billManager;

        //Sponsored Bills
        private int _sponsoredBillsCurrentPage = 1;
        private bool _sponsoredBillsIsThereMoreContent = false;
        private List<Bill> _sponsoredBills;
        protected RecyclerView.Adapter _sponsoredBillsRecyclerAdapter;

        //Cosponsored Bills
        private int _cosponsoredBillsCurrentPage = 1;
        private bool _cosponsoredBillsIsThereMoreContent = false;
        private List<Bill> _cosponsoredBills;
        //private BillManager _cosponsoredBillManager;
        protected RecyclerView.Adapter _cosponsoredBillsRecyclerAdapter;

        //committee
        private List<Committee> _committees;
        private CommitteeManager _committeeManager;
        protected RecyclerView.Adapter _committeeRecyclerAdapter;

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

            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

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
            _legistorManager = new LegislatorManager(MyLogger);
            //ls = new Shared.BusinessLayer.Services.LegislatorSvc(MyLogger);
            _voteManager = new VoteManager(MyLogger);
            _billManager = new BillManager(MyLogger);
            _committeeManager = new CommitteeManager(MyLogger);
            _viewPagerAdapter = new LegislatorViewPagerAdapter(ChildFragmentManager, _legislator);
        }

        public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;
            _selectableItemBackground = AppHelper.GetTypedValueFromActv(Activity);

            var fragmentView = inflater.Inflate(Resource.Layout.frag_ViewLegislator, container, false);

            PopulateBasicInfo(fragmentView, _legislator);
            PopulateContactMethodsButtons(fragmentView, _legislator);
            PopulateViewPager(fragmentView, _legislator, savedInstanceState);

            return fragmentView;
        }

        public override void OnStart()
        {
            base.OnStart();

            //SetPortrait(_legislator);
            GetBaseActivity().UpdateTitleBarText(_legislator.FullName());
        }

        public override void OnResume()
        {
            base.OnResume();

            if(portraitAsBitmap == null)
                SetPortrait(_legislator);
            else if(_portrait != null)
                _portrait.SetImageBitmap(portraitAsBitmap);
        }

        private void PopulateViewPager(View fragmentView, Legislator legislator, Bundle savedInstanceState)
        {
            var viewPager = fragmentView.FindViewById<ViewPager>(Resource.Id.viewLegislatorFrag_viewPager);
            viewPager.Adapter = _viewPagerAdapter;
            viewPager.CurrentItem = 1;

            //Set the number of pages that should be retained to either 
            //side of the current page in the view hierarchy in an idle 
            //state. Pages beyond this limit will be recreated from the 
            //adapter when needed.
            //viewPager.OffscreenPageLimit = 3;


            //GetVoteViewFrag().SetOnClickListener((vc) => FetchLegislatorVotes(true));
            //PopulateVote(savedInstanceState);

            //GetBillsSponsoredViewFrag().SetOnClickListener((bc) => FetchLegislatorBills(true, BillViewerKind.SponsoredBills));
            //PopulateSponsoredBills(savedInstanceState);

            //GetBillsCosponsoredViewFrag().SetOnClickListener((cc) => FetchLegislatorBills(true, BillViewerKind.CosponsoredBills));
            //PopulateCosponsoredBills(savedInstanceState);

            //PopulateCommittees(savedInstanceState);
        }

        /*
        private void PopulateVote(Bundle savedInstanceState)
        {
            if(savedInstanceState != null)
            {
                _votesIsThereMoreContent = savedInstanceState.GetBoolean(BundleType.VotesIsThereMoreContent, false);
                _votesCurrentPage = savedInstanceState.GetInt(BundleType.VotesCurrentPage, 1);

                if(!string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Votes, string.Empty)))
                {
                    var serializedVotes = savedInstanceState.GetString(BundleType.Votes);
                    _votes = new List<Vote>().DeserializeFromJson(serializedVotes) ?? null;
                }
            }

            if (_votes != null)
                GetVoteViewFrag()?.SetVotes(_votes, _votesIsThereMoreContent);

            else
                FetchLegislatorVotes(false);            
        }
        */

        /*
        private void PopulateSponsoredBills(Bundle savedInstanceState)
        {
            if (savedInstanceState != null)
            {
                _sponsoredBillsIsThereMoreContent = savedInstanceState.GetBoolean(BundleType.SponsoredBillsIsThereMoreContent, false);
                _sponsoredBillsCurrentPage = savedInstanceState.GetInt(BundleType.SponsoredBillsCurrentPage, 1);

                if (!string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.SponsoredBills, string.Empty)))
                {
                    var sponsoredBills = savedInstanceState.GetString(BundleType.SponsoredBills);
                    _sponsoredBills = new List<Bill>().DeserializeFromJson(sponsoredBills) ?? null;
                }
            }


            if (_sponsoredBills != null)
                GetBillsSponsoredViewFrag()?.SetBills(_sponsoredBills, _sponsoredBillsIsThereMoreContent);

            else
                FetchLegislatorBills(false, BillViewerKind.SponsoredBills);
        }

        private void PopulateCosponsoredBills(Bundle savedInstanceState)
        {
            if (savedInstanceState != null)
            {
                _cosponsoredBillsIsThereMoreContent = savedInstanceState.GetBoolean(BundleType.CosponsoredBillsIsThereMoreContent, false);
                _cosponsoredBillsCurrentPage = savedInstanceState.GetInt(BundleType.CosponsoredBillsCurrentPage, 1);

                if (!string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.CosponsoredBills, string.Empty)))
                {
                    var cosponsoredBills = savedInstanceState.GetString(BundleType.CosponsoredBills);
                    _cosponsoredBills = new List<Bill>().DeserializeFromJson(cosponsoredBills) ?? null;
                }
            }

            if (_cosponsoredBills != null)
                GetBillsCosponsoredViewFrag()?.SetBills(_cosponsoredBills, _cosponsoredBillsIsThereMoreContent);

            else
                FetchLegislatorBills(false, BillViewerKind.CosponsoredBills);
        }

        private void PopulateCommittees(Bundle savedInstanceState)
        {
            if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Committees, string.Empty)))
            {
                var serializedcomittees = savedInstanceState.GetString(BundleType.Committees);
                _committees = new List<Committee>().DeserializeFromJson(serializedcomittees) ?? null;
            }

            if (_committees != null)// && _votes.Count > 0)
                GetCommitteeViewFrag()?.SetCommittees(_committees);

            else
                FetchLegislatorCommittes();
        }
        */

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(BundleType.VotesCurrentPage, _votesCurrentPage);
            outState.PutBoolean(BundleType.VotesIsThereMoreContent, _votesIsThereMoreContent);

            outState.PutInt(BundleType.SponsoredBillsCurrentPage, _sponsoredBillsCurrentPage);
            outState.PutBoolean(BundleType.SponsoredBillsIsThereMoreContent, _sponsoredBillsIsThereMoreContent);

            outState.PutInt(BundleType.CosponsoredBillsCurrentPage, _cosponsoredBillsCurrentPage);
            outState.PutBoolean(BundleType.CosponsoredBillsIsThereMoreContent, _cosponsoredBillsIsThereMoreContent);

            if (_votes != null)
            {
                var serializedVotes = _votes.SerializeToJson();
                outState.PutString(BundleType.Votes, serializedVotes);
            }

            if(_sponsoredBills != null)
            {
                var serializedSponsoredBills = _sponsoredBills.SerializeToJson();
                outState.PutString(BundleType.SponsoredBills, serializedSponsoredBills);
            }

            if (_cosponsoredBills != null)
            {
                var serializedcosponsoredBills = _cosponsoredBills.SerializeToJson();
                outState.PutString(BundleType.CosponsoredBills, serializedcosponsoredBills);
            }

            if (_committees != null)
            {
                var serializedCommittees = _committees.SerializeToJson();
                outState.PutString(BundleType.Committees, serializedCommittees);
            }
        }

        private VoteViewerFragmentCtrl GetVoteViewFrag()
        {
            //return _viewPagerAdapter.GetFragment(ViewPagerList.LegislatorVotes) as VoteViewerFragmentCtrl;
            return _viewPagerAdapter.GetExitsingItem(ViewPagerList.LegislatorVotes) as VoteViewerFragmentCtrl;
        }

        private CommitteeViewerFragmentCtrl GetCommitteeViewFrag()
        {
            //return _viewPagerAdapter.GetFragment(ViewPagerList.LegislatorCommittees) as CommitteeViewerFragmentCtrl;
            return _viewPagerAdapter.GetExitsingItem(ViewPagerList.LegislatorCommittees) as CommitteeViewerFragmentCtrl;
        }

        private BillViewerFragmentCtrl GetBillsSponsoredViewFrag()
        {
            //return _viewPagerAdapter.GetFragment(ViewPagerList.LegislatorBillsSponsored) as BillViewerFragmentCtrl;
            return _viewPagerAdapter.GetExitsingItem(ViewPagerList.LegislatorBillsSponsored) as BillViewerFragmentCtrl;
        }

        private BillViewerFragmentCtrl GetBillsCosponsoredViewFrag()
        {
            return _viewPagerAdapter.GetExitsingItem(ViewPagerList.LegislatorBillsCosponsored) as BillViewerFragmentCtrl;
        }

        protected void FetchLegislatorCommittes()
        {
            var getCommitteesTask = new Task<List<Committee>>((prms) =>
            {
                var passedParams = prms as Tuple<string, CommitteeManager>;

                var legislatorId = passedParams.Item1;
                var cm = passedParams.Item2;

                return cm.GetCommitteesForLegislator(legislatorId);
            }, new Tuple<string, CommitteeManager>(_legislator.IdBioguide, _committeeManager));

            getCommitteesTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    _committees = antecedent.Result;
                    GetCommitteeViewFrag()?.ShowCommittees(_committees);
                });
            });
            getCommitteesTask.Start();
        }

        protected void FetchLegislatorVotes(bool isNextClick)
        {
            if (isNextClick)
            {
                _votesCurrentPage++;
                GetVoteViewFrag()?.SetLoadMoreButtonTextAsLoading(true);
            }

            var getVotesTask = new Task<Tuple<List<Vote>, bool>>((prms) =>
            {
                var passedParams = prms as Tuple<string, VoteManager, int>;

                var legislatorId = passedParams.Item1;
                var vm = passedParams.Item2;
                var currentPage = passedParams.Item3;

                var results = vm.GetLegislatorVotes(legislatorId, currentPage);
                var isThereMoreVotes = vm.IsThereMoreResultsForLastCall();

                return new Tuple<List<Vote>, bool>(results, isThereMoreVotes);
            }, new Tuple<string, VoteManager, int>(_legislator.IdBioguide, _voteManager, _votesCurrentPage));

            getVotesTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    _votesIsThereMoreContent = antecedent.Result.Item2;

                    if (_votes == null || !_votesIsThereMoreContent)
                        _votes = antecedent.Result.Item1;
                    else
                        _votes.AddRange(antecedent.Result.Item1);

                    if(GetVoteViewFrag() != null)
                        GetVoteViewFrag().ShowVotes(_votes, _votesIsThereMoreContent);

                });
            });
            getVotesTask.Start();
        }

        protected void FetchLegislatorBills(bool isNextClick, BillViewerKind billViewerKind)
        {
            int currentPage = 1;
            
            if (isNextClick)
            {
                if (billViewerKind == BillViewerKind.SponsoredBills)
                {
                    _sponsoredBillsCurrentPage++;
                    currentPage = _sponsoredBillsCurrentPage;
                    GetBillsSponsoredViewFrag()?.SetLoadMoreButtonTextAsLoading(true);

                    if (GetBillsSponsoredViewFrag() != null)
                        GetBillsSponsoredViewFrag().ShowBills(_sponsoredBills, _sponsoredBillsIsThereMoreContent);
                }
                else if(billViewerKind == BillViewerKind.CosponsoredBills)
                {
                    _cosponsoredBillsCurrentPage++;
                    currentPage = _cosponsoredBillsCurrentPage;

                    if(GetBillsCosponsoredViewFrag() != null)
                        GetBillsCosponsoredViewFrag().ShowBills(_cosponsoredBills, _cosponsoredBillsIsThereMoreContent);
                }
            }

            var getBillsTask = new Task<Tuple<List<Bill>, bool, BillViewerKind>>((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName)); 
                var currentPageVal = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = mode == BillViewerKind.CosponsoredBills
                    ? bm.GetBillsCosponsoredbyLegislator(legislatorId, currentPageVal)
                    : bm.GetBillsSponsoredbyLegislator(legislatorId, currentPageVal);

                var isThereMoreVotes = bm.IsThereMoreResultsForLastCall();

                return new Tuple<List<Bill>, bool, BillViewerKind>(results, isThereMoreVotes, mode);
            }, new Tuple<string, BillManager, int, int>(_legislator.IdBioguide, _billManager, currentPage, (int)billViewerKind));

            getBillsTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    BillViewerKind mode =  antecedent.Result.Item3;

                    var isThereMoreBills = antecedent.Result.Item2;
                    List<Bill> currentBillsList = mode == BillViewerKind.SponsoredBills
                        ? _sponsoredBills
                        : _cosponsoredBills;

                    if(currentBillsList == null || !isThereMoreBills)
                        currentBillsList = antecedent.Result.Item1;
                    else
                        currentBillsList.AddRange(antecedent.Result.Item1);

                    if(mode == BillViewerKind.SponsoredBills)
                    {
                        _sponsoredBillsIsThereMoreContent = isThereMoreBills;
                        GetBillsSponsoredViewFrag()?.ShowBills(currentBillsList, isThereMoreBills);
                    }
                    else
                    {
                        _cosponsoredBillsIsThereMoreContent = isThereMoreBills;
                        GetBillsCosponsoredViewFrag()?.ShowBills(currentBillsList, isThereMoreBills);
                    }
                });
            });

            getBillsTask.Start();
        }

        private void PopulateBasicInfo(View fragment, Legislator legislator)
        {
            _portrait = fragment.FindViewById<ImageView>(Resource.Id.viewLegislatorFrag_portrait);
            AppHelper.SetLegislatorPortrait(legislator, _portrait);

            using (var chamber = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_chamber))
                chamber.Text = $"{AndroidHelper.GetString(Resource.String.chamber)}: {legislator.Chamber} ({legislator.State.ToString()})";

            using (var party = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_party))
                party.Text = $"{AndroidHelper.GetString(Resource.String.party)}: {legislator.Party.ToString()}";

            var termStartDateText = AndroidHelper.GetString(Resource.String.termStarted);
            using (var termStartDate = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_termStartDate))
                termStartDate.Text = AppHelper.GetLegislatorTermStartDate(legislator, termStartDateText);

            var termEndDateText = AndroidHelper.GetString(Resource.String.termEnds);
            using (var termEndDate = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_termEndDate))
                termEndDate.Text = AppHelper.GetLegislatorTermEndDate(legislator, termEndDateText);

            using (var birthday = fragment.FindViewById<TextView>(Resource.Id.viewLegislatorFrag_birthdate))
            {
                var bdayText = AndroidHelper.GetString(Resource.String.birthday);
                birthday.Text = legislator.Birthday == DateTime.MinValue
                    ? string.Empty
                    : $"{bdayText}: {AppHelper.GetLegislatorBirthdateAndAge(legislator)}";
            }
        }

        private void SetPortrait(Legislator legislator)
        {
            var getLegislatorPortraitTask = new Task<byte[]>((prms) =>
            {
                var passedParams = prms as Tuple<string, LegislatorManager>;

                var legislatorId = passedParams.Item1;
                var lm = passedParams.Item2;

                var portraitAsByteArray = lm.GetLegislatorPortraitAsByteArray(legislatorId);

                return portraitAsByteArray;
            }, new Tuple<string, LegislatorManager>(_legislator.IdBioguide, _legistorManager));

            getLegislatorPortraitTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    if (!antecedent.IsFaulted || !antecedent.IsCanceled)
                    {
                        try
                        {
                            var portraitByteArray = antecedent.Result;
                            if (portraitByteArray != null && portraitByteArray.Length > 0 && Activity != null && !Activity.IsFinishing)
                            {
								
                                portraitAsBitmap = Android.Graphics.BitmapFactory.DecodeByteArray(portraitByteArray, 0, portraitByteArray.Length);

                                Activity.RunOnUiThread(
                                    () => _portrait.SetImageBitmap(portraitAsBitmap));
                            }

                        }
                        catch (Exception e)
                        {
                            MyLogger.Error("Error retrieving and setting legislator portrait", e);
                        }
                    }
                });
            });

            getLegislatorPortraitTask.Start();

        }

        private void PopulateContactMethodsButtons(View fragment, Legislator legislator)
        {
            var WriteLetter = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_writeLetter);
            var Email = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_email);
            var Phone = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_phone);
            var Address = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_address);

            var Facebook = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_facebook);
            var Twitter = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_twitter);
            var Webpage = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_webpage);
            var YouTube = fragment.FindViewById<ImageButton>(Resource.Id.viewLegislatorFrag_youtube);

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
            AppHelper.SetLegislatorContactMthdVisibility(button, contactMethod, _selectableItemBackground);
            //button.Visibility = contactMethod.IsEmpty
            //    ? ViewStates.Gone
            //    : ViewStates.Visible;

            button.Click += (sender, e) => ContactButton_Click(contactMethod);
        }

        private void ContactButton_Click(ContactMethod contactMethod)
        {
            AppHelper.PerformContactMethodIntent(this as BaseFragment, contactMethod, false);
        }
    }
}