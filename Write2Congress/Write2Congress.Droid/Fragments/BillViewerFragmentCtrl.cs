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
        private List<Bill> _bills = new List<Bill>();
        private Legislator _legislator;
        private BillViewerKind _viewerMode;

        public BillViewerFragmentCtrl() { }

        public static BillViewerFragmentCtrl CreateInstance(Legislator legislator, BillViewerKind viewerMode)
        {
            var newFragment = new BillViewerFragmentCtrl();

            var args = new Bundle();
            args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            args.PutInt(BundleType.BillViewerFragmentType, (int)viewerMode);
            newFragment.Arguments = args;

            return newFragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);
            _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);
            _viewerMode = (BillViewerKind)Arguments.GetInt(BundleType.BillViewerFragmentType);

            _billManager = new BillManager(MyLogger);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (savedInstanceState != null && savedInstanceState.ContainsKey(BundleType.BillViewerFragmentType))
                _viewerMode = (BillViewerKind)savedInstanceState.GetInt(BundleType.BillViewerFragmentType);

            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new BillAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();
            RetrieveCurrentPageIfAvailable(savedInstanceState);

            if (_bills != null && _bills.Count > 0)
                UpdateBills(_bills);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Bills, string.Empty)))
            {
                var serializedBills = savedInstanceState.GetString(BundleType.Bills);
                _bills = new List<Bill>().DeserializeFromJson(serializedBills);
                UpdateBills(_bills);
            }
            else
                FetchMoreLegislatorContent(false);

            return fragment;
        }

        protected override void FetchMoreLegislatorContent(bool isNextClick)
        {
            base.FetchMoreLegislatorContent(isNextClick);

            var getBillsTask = new Task< Tuple<List<Bill>, bool> >((prms) =>
            {
                var passedParams = (prms as Tuple<string, BillManager, int, int>);

                var legislatorId = passedParams.Item1;
                var bm = new BillManager(new Logger(Class.SimpleName));  //passedParams.Item2;
                var currentPage = passedParams.Item3;
                var mode = (BillViewerKind)((int)passedParams.Item4);

                var results = mode == BillViewerKind.CosponsoredBills
                    ? bm.GetBillsCosponsoredbyLegislator(legislatorId, currentPage)
                    : bm.GetBillsSponsoredbyLegislator(legislatorId, currentPage);

                var isThereMoreVotes = bm.IsThereMoreResultsForLastCall();

                return new Tuple<List<Bill>, bool>(results, isThereMoreVotes);
            }, new Tuple<string, BillManager, int, int>(_legislator.BioguideId, _billManager, currentPage, (int)_viewerMode));

            getBillsTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    var isThereMoreVotes = antecedent.Result.Item2;

                    if (isThereMoreVotes)
                        _bills.AddRange(antecedent.Result.Item1);
                    else
                        _bills = antecedent.Result.Item1;

                    SetLoadMoreButtonAsLoading(false);
                    ShowRecyclerButtons(isThereMoreVotes);
                    UpdateBills(_bills);
                });
            });

            getBillsTask.Start();
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

        public void UpdateBills(List<Bill> bills)
        {
            (recyclerAdapter as BillAdapter).UpdateBill(bills);
            SetLoadingUiOff();
        }
    }
}