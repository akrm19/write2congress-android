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
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.Adapters;
using System.Threading.Tasks;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Fragments
{
    public class BillViewerFragmentCtrl : BaseRecyclerViewerFragment
    {
        private BillManager _billManager;
        private bool _isThereMoreVotes = true;
        private List<Bill> _bills;
        private Legislator _legislator;
        private BillViewerKind _viewerMode;

        public BillViewerFragmentCtrl() { }

        public static BillViewerFragmentCtrl CreateInstance(Legislator legislator, BillViewerKind viewerMode)
        {
            var newFragment = new BillViewerFragmentCtrl();

			var args = new Bundle();

            if (legislator != null)
                args.PutString(BundleType.Legislator, legislator.SerializeToJson());

            args.PutInt(BundleType.BillViewerFragmentType, (int)viewerMode);

			newFragment.Arguments = args;
            return newFragment;
        }

        public static BillViewerFragmentCtrl CreateInstance(BillViewerKind viewerMode)
        {
            return CreateInstance(null, viewerMode);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			var serialziedLegislator = Arguments.GetString(BundleType.Legislator);

            if(!string.IsNullOrWhiteSpace(serialziedLegislator))
			    _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

            _billManager = new BillManager(MyLogger);
            _viewerMode = (BillViewerKind)Arguments.GetInt(BundleType.BillViewerFragmentType);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(BundleType.BillViewerFragmentType))
                _viewerMode = (BillViewerKind)savedInstanceState.GetInt(BundleType.BillViewerFragmentType);

            if(_legislator == null)
				_legislator = RetrieveLegislatorIfAvailable(savedInstanceState);

            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new BillAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();

            if (_bills != null && _bills.Count >= 0)
                SetBills(_bills, _isThereMoreVotes);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Bills, string.Empty)))
            {
                var serializedBills = savedInstanceState.GetString(BundleType.Bills);
                _bills = new List<Bill>().DeserializeFromJson(serializedBills);
                SetBills(_bills, _isThereMoreVotes);
            }
            else
                FetchMoreLegislatorContent(false);

            return fragment;
        }

        protected override void FetchMoreLegislatorContent(bool isNextClick)
        {
            base.FetchMoreLegislatorContent(isNextClick);
            var getBillsTask = (_viewerMode == BillViewerKind.AllBillsOfEveryone)
                ? GetAllBillsTasks()
                : GetLegislatorsBillsTask();
            /*
            var getBillsTask = new Task< Tuple<List<Bill>, bool, int> >((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName));
                var localCurrentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = mode == BillViewerKind.CosponsoredBills
                    ? bm.GetBillsCosponsoredbyLegislator2(legislatorId, localCurrentPage)
                    : bm.GetBillsSponsoredbyLegislator2(legislatorId, localCurrentPage);

                var isThereMoreVotes = results.IsThereMoreResults;

                return new Tuple<List<Bill>, bool, int>(results.Results, isThereMoreVotes, localCurrentPage);
            }, new Tuple<string, BillManager, int, int>(_legislator.IdBioguide, _billManager, currentPage, (int)_viewerMode));
            */
            
            getBillsTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    if (antecedent.IsFaulted || antecedent.IsCanceled)
                    {
                        HandleErrorRetrievingData();
                    }
                    else
                    {
                        HandleSuccessfullDataRetrieval();

                        currentPage = antecedent.Result.Item3 + 1;
                        _isThereMoreVotes = antecedent.Result.Item2;

                        if (_bills == null || !_bills.Any())
                            _bills = antecedent.Result.Item1;
                        else
                            _bills.AddRange(antecedent.Result.Item1);

                        SetLoadMoreButtonTextAsLoading(false);
                        ShowRecyclerButtons(_isThereMoreVotes);
                        ShowBills(_bills, _isThereMoreVotes);
                    }
                });
            });

            getBillsTask.Start();
        }

        private Task<Tuple<List<Bill>, bool, int>> GetLegislatorsBillsTask()
        {
            var getBillsTask = new Task<Tuple<List<Bill>, bool, int>>((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName));
                var localCurrentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = mode == BillViewerKind.CosponsoredBills
                    ? bm.GetBillsCosponsoredbyLegislator2(legislatorId, localCurrentPage)
                    : bm.GetBillsSponsoredbyLegislator2(legislatorId, localCurrentPage);

                var isThereMoreVotes = results.IsThereMoreResults;

                return new Tuple<List<Bill>, bool, int>(results.Results, isThereMoreVotes, localCurrentPage);
            }, new Tuple<string, BillManager, int, int>(_legislator.IdBioguide, _billManager, currentPage, (int)_viewerMode));

            return getBillsTask;
        }

        private Task<Tuple<List<Bill>, bool, int>> GetAllBillsTasks()
        {
            var getBillsTask = new Task<Tuple<List<Bill>, bool, int>>((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName));
                var localCurrentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = bm.GetBillsIntroduced(localCurrentPage);

                var isThereMoreVotes = results.IsThereMoreResults;

                return new Tuple<List<Bill>, bool, int>(results.Results, isThereMoreVotes, localCurrentPage);
            }, new Tuple<string, BillManager, int, int>("search term goes here", _billManager, currentPage, (int)_viewerMode));

            return getBillsTask;
        }


        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (_bills != null)
            {
                var serializedBills = _bills.SerializeToJson();
                outState.PutString(BundleType.Bills, serializedBills);
            }

            outState.PutInt(BundleType.BillViewerFragmentType, (int)_viewerMode);
            
            outState.PutBoolean(_viewerMode == BillViewerKind.SponsoredBills 
                ? BundleType.SponsoredBillsIsThereMoreContent
                : BundleType.CosponsoredBillsIsThereMoreContent, _isThereMoreVotes);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (errorOccurred)
                HandleErrorRetrievingData();
            else if (_bills == null)
                SetLoadingUi();
            else
                ShowBills(_bills, _isThereMoreVotes);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _billManager = null;
            _bills = null;
            _legislator = null;
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyBillsText);
        }

        public override string ViewerTitle()
        {
            switch (_viewerMode)
            {
                default:
                case BillViewerKind.SponsoredBills:
                    return AndroidHelper.GetString(Resource.String.billsSponsored);
                case BillViewerKind.CosponsoredBills:
                    return AndroidHelper.GetString(Resource.String.billsCosponsored);
            }
        }

        public void SetBills(List<Bill> bills, bool isThereMoreVotes)
        {
            _bills = bills;
            _isThereMoreVotes = isThereMoreVotes;
        }

        public void ShowBills(List<Bill> bills, bool isThereMoreVotes)
        {
            SetBills(bills, isThereMoreVotes);

            if (IsBeingShown)
            {
                SetLoadMoreButtonTextAsLoading(false);
                ShowRecyclerButtons(_isThereMoreVotes);

                (recyclerAdapter as BillAdapter).UpdateBill(bills);
                SetLoadingUiOff();
            }
        }
    }
}