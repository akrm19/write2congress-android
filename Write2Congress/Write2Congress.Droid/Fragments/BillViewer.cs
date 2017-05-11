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

namespace Write2Congress.Droid.Fragments
{
    public class BillViewer : BaseRecyclerViewerFragment
    {
        private BillManager _billManager;
        private List<Bill> _bills;
        private Legislator _legislator;

        public BillViewer() { }

        public static BillViewer CreateInstance(Legislator legislator)
        {
            var newFragment = new BillViewer();

            var args = new Bundle();
            args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            newFragment.Arguments = args;

            return newFragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);
            _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

            _billManager = new BillManager(MyLogger);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
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
                    var legislatorId = (prms as Tuple<string, BillManager>).Item1;
                    var bm = (prms as Tuple<string, BillManager>).Item2;

                    return bm.GetBillsSponsoredbyLegislator(_legislator.BioguideId, 1);
                }, new Tuple<string, BillManager>(_legislator.BioguideId, _billManager));

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
            return AndroidHelper.GetString(Resource.String.billsSponsored);
        }
    }
}