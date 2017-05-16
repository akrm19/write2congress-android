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
        private List<Bill> _bills;
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

            if (_bills != null && _bills.Count > 0)
                UpdateBills(_bills);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Bills, string.Empty)))
            {
                var serializedBills = savedInstanceState.GetString(BundleType.Bills);
                _bills = new List<Bill>().DeserializeFromJson(serializedBills);
                UpdateBills(_bills);
            }
            else
            {
                var getBillsTask = new Task<List<Bill>>((prms) =>
                {
                    var paramsTuple = (prms as Tuple<string, BillManager, int>);
                    var legislatorId = paramsTuple.Item1;
                    var bm = paramsTuple.Item2;
                    var mode = (BillViewerKind)((int)paramsTuple.Item3);

                    return mode == BillViewerKind.CosponsoredBills
                        ? bm.GetBillsCosponsoredbyLegislator(legislatorId, 1)
                        : bm.GetBillsSponsoredbyLegislator(legislatorId, 1);

                }, new Tuple<string, BillManager, int>(_legislator.BioguideId, _billManager, (int)_viewerMode));

                getBillsTask.ContinueWith((antecedent) =>
                {
                    if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                        return;

                    Activity.RunOnUiThread(() =>
                    {
                        _bills = antecedent.Result;
                        UpdateBills(_bills);
                    });
                });

                getBillsTask.Start();
            }
            return fragment;
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

        public void UpdateBills(List<Bill> bills)
        {
            (recyclerAdapter as BillAdapter).UpdateBill(bills);
            SetLoadingUiOff();
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
    }
}