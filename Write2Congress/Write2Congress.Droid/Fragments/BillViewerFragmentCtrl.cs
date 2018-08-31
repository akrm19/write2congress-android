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
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.Fragments
{
    public class BillViewerFragmentCtrl : BaseRecyclerViewerFragment
    {
        private BillManager _billManager;
        private bool _isThereMoreVotes = true;
        private List<Bill> _billsToDisplay;
        private Legislator _legislator;
        private BillViewerKind _viewerMode;
        private string _lastSearchTerm;

        public BillViewerFragmentCtrl() { }

        public static BillViewerFragmentCtrl CreateInstance(BillViewerKind viewerMode)
		{
			return CreateInstance(null, viewerMode);
		}

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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			var serialziedLegislator = Arguments.GetString(BundleType.Legislator);

            if(!string.IsNullOrWhiteSpace(serialziedLegislator))
			    _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

            if (Arguments.ContainsKey(BundleType.BillsIsThereMoreContent))
                _isThereMoreVotes = Arguments.GetBoolean(BundleType.BillsIsThereMoreContent);
			
			_billManager = new BillManager(MyLogger);
			_viewerMode = (BillViewerKind)Arguments.GetInt(BundleType.BillViewerFragmentType);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(BundleType.BillViewerFragmentType))
                _viewerMode = (BillViewerKind)savedInstanceState.GetInt(BundleType.BillViewerFragmentType);

            if(_legislator == null && (_viewerMode != BillViewerKind.BillSearch && _viewerMode != BillViewerKind.LastestBillsForEveryone))
				_legislator = RetrieveLegislatorIfAvailable(savedInstanceState);

            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            //recyclerAdapter = new BillAdapter(this);
            //recycler.SetAdapter(recyclerAdapter);
            recycler.SetAdapter(new BillAdapter(this));

            //recycler.GetAdapter();

            if (_viewerMode == BillViewerKind.LastestBillsForEveryone)
                HookupToolbarEventsForBillsFiltering();
            else if (_viewerMode == BillViewerKind.BillSearch)
            {
                HookupToolbarEventsForBillsFiltering();
                HookupToolbarEventsForBillSearch();
            }

            SetLoadingTextInEmptyView();

            if (_billsToDisplay != null && _billsToDisplay.Count >= 0)
                SetBills(_billsToDisplay, _isThereMoreVotes);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Bills, string.Empty)))
            {
                var serializedBills = savedInstanceState.GetString(BundleType.Bills);
                _billsToDisplay = new List<Bill>().DeserializeFromJson(serializedBills);
                SetBills(_billsToDisplay, _isThereMoreVotes);
            }
            // Removing '|| !string.IsNullOrWhiteSpace(_lastSearchTerm))' for now
            // since it will fetch legislator content when searchview is not empty and the 
            // use does something like switch orientation
            else if (_viewerMode != BillViewerKind.BillSearch)// || !string.IsNullOrWhiteSpace(_lastSearchTerm))
                FetchMoreLegislatorContent(false);

            return fragment;
        }

        private void HookupToolbarEventsForBillsFiltering()
        {
            GetBaseActivityWithToolbarSearch().FilterSearchviewCollapsed += HandleFilterMenuItemCollapsed;
            GetBaseActivityWithToolbarSearch().FilterSearchTextChanged += FilterBills;
        }

        private void HookupToolbarEventsForBillSearch()
        {
            GetBaseActivityWithToolbarSearch().SearchSearchviewCollapsed += HandleSearchMenuItemCollapsed;
            GetBaseActivityWithToolbarSearch().SearchQuerySubmitted += FetchBillsSearchResults;
            GetBaseActivityWithToolbarSearch().ExitSearchClicked += HandleExitSearchviewClicked;
        }

        private void DisconnectToolbarEvents()
        {
            GetBaseActivityWithToolbarSearch().FilterSearchviewCollapsed -= HandleFilterMenuItemCollapsed;
            GetBaseActivityWithToolbarSearch().FilterSearchTextChanged -= FilterBills;
            GetBaseActivityWithToolbarSearch().SearchSearchviewCollapsed -= HandleSearchMenuItemCollapsed;
            GetBaseActivityWithToolbarSearch().SearchQuerySubmitted -= FetchBillsSearchResults;
            GetBaseActivityWithToolbarSearch().ExitSearchClicked -= HandleExitSearchviewClicked;
        }

        public void FilterBills(string filter)
        {
            try
            {
                var filteredBills = _billManager.FilterBillsByQuery(_billsToDisplay, filter);
                //(recyclerAdapter as BillAdapter).UpdateBill(filteredBills);
                (recycler.GetAdapter() as BillAdapter).UpdateBill(filteredBills);
            }
            catch(Exception e)
            {
                MyLogger.Error("An error occured filtering bills in BillViewer", e);
            }
        }

        //used on search click 
        protected void FetchBillsSearchResults(string searchTerm)
        {
			currentPage = 1;
			_billsToDisplay = null;       
			_lastSearchTerm = searchTerm;

			//TODO RM: SHOW EMPTY EVIEW THEN RETURN
			if (string.IsNullOrWhiteSpace(searchTerm))
				return;
            
            //FetchBillsSearchResults(searchTerm, false);
            FetchMoreLegislatorContent(false);
        }

        /*
        protected void FetchBillsSearchResults(string searchTerm, bool isLoadMoreClick)
        {
            //_viewerMode = BillViewerKind.BillSearch;


            base.FetchMoreLegislatorContent(isLoadMoreClick);
            var getBillsTask = GetBillsSearchTask(searchTerm, isLoadMoreClick);

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

                        if (_billsToDisplay == null || !_billsToDisplay.Any())
                            _billsToDisplay = antecedent.Result.Item1;
                        else
                            _billsToDisplay.AddRange(antecedent.Result.Item1);


                        SetLoadMoreButtonTextAsLoading(false);
                        ShowRecyclerButtons(_isThereMoreVotes);
                        ShowBills(_billsToDisplay, _isThereMoreVotes);

                        var newTitle = $"'{antecedent.Result.Item4}' Bills";
                        GetBaseActivity().UpdateTitleBarText(newTitle);
                    }

                    SetToolbarForSearchResultReturned();
                });
            });

            getBillsTask.Start();
        }
        */

        protected void SetToolbarForSearchResultReturned()
        {
            GetBaseActivityWithToolbarSearch().CollapseToolbarSearchview();
			GetBaseActivityWithToolbarSearch().SetToolbarFilterviewVisibility(true);
            GetBaseActivityWithToolbarSearch().SetToolbarExitSearchviewVisibility(true);
            GetBaseActivityWithToolbarSearch().SetToolbarSearchviewVisibility(false);
        }

        private void HandleExitSearchviewClicked()
        {
            _billsToDisplay = null;
            _lastSearchTerm = string.Empty;
            currentPage = 1;


            GetBaseActivity().UpdateTitleBarText(AndroidHelper.GetString(Resource.String.searchBills));

            ShowBills(_billsToDisplay, _isThereMoreVotes);
        }

        private void HandleSearchMenuItemCollapsed()
        {
            GetBaseActivityWithToolbarSearch().SetToolbarFilterviewVisibility(true);
        }

        private void HandleFilterMenuItemCollapsed()
        {
            GetBaseActivityWithToolbarSearch().SetToolbarExitSearchviewVisibility(true);
            GetBaseActivityWithToolbarSearch().SetToolbarSearchviewVisibility(false);
        }

        protected override void FetchMoreLegislatorContent(bool isNextClick)
        {
            base.FetchMoreLegislatorContent(isNextClick);

            var getBillsTask = GetBillsContentTaskForViewerMode(_viewerMode);

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

                        if (_billsToDisplay == null || !_billsToDisplay.Any())
                            _billsToDisplay = antecedent.Result.Item1;
                        else
                            _billsToDisplay.AddRange(antecedent.Result.Item1);

                        SetLoadMoreButtonTextAsLoading(false);
                        ShowRecyclerButtons(_isThereMoreVotes);
                        ShowBills(_billsToDisplay, _isThereMoreVotes);

                        if(_viewerMode == BillViewerKind.LastestBillsForEveryone)
                            GetBaseActivity().UpdateTitleBarText(AndroidHelper.GetString(Resource.String.latestBills));
                        else if(_viewerMode == BillViewerKind.BillSearch)
                            GetBaseActivity().UpdateTitleBarText($"'{antecedent.Result.Item4}' Bills");
                    }

                    if (_viewerMode == BillViewerKind.BillSearch)
                        SetToolbarForSearchResultReturned();
                });
            });

            getBillsTask.Start();
        }

        private Task<Tuple<List<Bill>, bool, int, string>> GetBillsContentTaskForViewerMode(BillViewerKind viewerKind)
        {
            switch (viewerKind)
            {
                //TODO RM: Fix the performance bug. It is caued by the 
                //data we get back from search. Not sure why, but maybe
                //it is the changes in the items

				case BillViewerKind.BillSearch:
                    return GetBillsSearchTask(_lastSearchTerm);
                case BillViewerKind.LastestBillsForEveryone:
                    return GetAllBillsTasks();
                case BillViewerKind.SponsoredBills:
                case BillViewerKind.CosponsoredBills:
                    return GetLegislatorsBillsTask();
                default:
                    return null;
            }
        }

        private Task<Tuple<List<Bill>, bool, int, string>> GetLegislatorsBillsTask()
        {
            var getBillsTask = new Task<Tuple<List<Bill>, bool, int, string>>((prms) =>
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

                return new Tuple<List<Bill>, bool, int, string>(results.Results, isThereMoreVotes, localCurrentPage, string.Empty);
            }, new Tuple<string, BillManager, int, int>(_legislator.IdBioguide, _billManager, currentPage, (int)_viewerMode));

            return getBillsTask;
        }

        private Task<Tuple<List<Bill>, bool, int, string>> GetAllBillsTasks()
        {
            var getBillsTask = new Task<Tuple<List<Bill>, bool, int, string>>((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName));
                var localCurrentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = bm.GetBillsIntroduced(localCurrentPage);

                var isThereMoreVotes = results.IsThereMoreResults;

                return new Tuple<List<Bill>, bool, int, string>(results.Results, isThereMoreVotes, localCurrentPage, string.Empty);
            }, new Tuple<string, BillManager, int, int>("search term goes here", _billManager, currentPage, (int)_viewerMode));

            return getBillsTask;
        }

        private Task<Tuple<List<Bill>, bool, int, string>> GetBillsSearchTask(string searchTerm)
        {
            var getBillsTask = new Task<Tuple<List<Bill>, bool, int, string>>((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var searchTermEntered = passedParams.Item1;
                var bm = passedParams.Item2;//new BillManager(new Logger(Class.SimpleName));
                var localCurrentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = bm.GetBillsBySubject(searchTermEntered, localCurrentPage);

                var isThereMoreVotes = results.IsThereMoreResults;

                return new Tuple<List<Bill>, bool, int, string>(results.Results, isThereMoreVotes, localCurrentPage, searchTermEntered);
            }, new Tuple<string, BillManager, int, int>(searchTerm, _billManager, currentPage, (int)_viewerMode));

            return getBillsTask;
        }


        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

			/* 
             * TODO RM: This change seems to prevent the crash, but there is still
             * issues with slowness
             * */
            if (_billsToDisplay != null)
            {
                var serializedBills = _billsToDisplay.SerializeToJson();
                outState.PutString(BundleType.Bills, serializedBills);
            }

            outState.PutInt(BundleType.BillViewerFragmentType, (int)_viewerMode);
            outState.PutBoolean(BundleType.BillsIsThereMoreContent, _isThereMoreVotes);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (errorOccurred)
                HandleErrorRetrievingData();
            else if (_viewerMode == BillViewerKind.BillSearch && string.IsNullOrWhiteSpace(_lastSearchTerm))
                ShowEmptyview(GetString(Resource.String.enterSearchCriteria));
            else if (_billsToDisplay == null)
                SetLoadingTextInEmptyView();
            else
                ShowBills(_billsToDisplay, _isThereMoreVotes);
        }

        protected override void CleanUp()
        {
            if(GetBaseActivityWithToolbarSearch() != null)
                DisconnectToolbarEvents();
            
            _billManager = null;
            _billsToDisplay = null;
            _legislator = null;

			base.CleanUp();
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyBillsText);
        }

        public override string ViewerTitle()
        {
            switch (_viewerMode)
            {
                case BillViewerKind.SponsoredBills:
                    return AndroidHelper.GetString(Resource.String.billsSponsored);
                case BillViewerKind.CosponsoredBills:
                    return AndroidHelper.GetString(Resource.String.billsCosponsored);
				default:
                    return string.Empty;
            }
        }

        public void SetBills(List<Bill> bills, bool isThereMoreVotes)
        {
            _billsToDisplay = bills;
            _isThereMoreVotes = isThereMoreVotes;
        }

        public void ShowBills(List<Bill> bills, bool isThereMoreVotes)
        {
            SetBills(bills, isThereMoreVotes);

            if (IsBeingShown)
            {
                SetLoadMoreButtonTextAsLoading(false);
                ShowRecyclerButtons(_isThereMoreVotes);

                //(recyclerAdapter as BillAdapter).UpdateBill(bills);
                (recycler.GetAdapter() as BillAdapter).UpdateBill(bills);
                SetLoadingUiOff();
            }
        }

        private IActivityWithToolbarSearch GetBaseActivityWithToolbarSearch()
        {
            return (GetBaseActivity() as IActivityWithToolbarSearch);
        }
    }
}