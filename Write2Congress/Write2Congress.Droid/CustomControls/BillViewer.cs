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
using Android.Util;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Adapters;
using Android.Support.V7.Widget;

namespace Write2Congress.Droid.CustomControls
{
    public class BillViewer : LinearLayout
    {
        private BillAdapter _billAdapter;
        private ViewSwitcher _viewSwitcher;
        private TextView _header, _emptyText;

        private BillManager _billManager;
        private List<Bill> _bills;
        private BaseFragment _fragment;
        private Logger _logger;

        public BillViewer(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BillViewer(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            _logger = new Logger(Class.SimpleName);
            _billManager = new BillManager(_logger);

            using (var layoutInflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater)
                layoutInflater.Inflate(Resource.Layout.ctrl_BillViewer, this, true);
        }

        public void SetupCtrl(BaseFragment fragment)
        {
            _fragment = fragment;

            _viewSwitcher = FindViewById<ViewSwitcher>(Resource.Id.billViewer_viewSwitcher);
            _header = FindViewById<TextView>(Resource.Id.billViewer_header);
            _emptyText = FindViewById<TextView>(Resource.Id.billViewer_emptyText);

            var layoutManager = new LinearLayoutManager(_fragment.Context, LinearLayoutManager.Vertical, false);
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.billViewer_recycler);
            recyclerView.SetLayoutManager(layoutManager);

            _billAdapter = new BillAdapter(_fragment);
            recyclerView.SetAdapter(_billAdapter);

            SetLoadingUi();
        }

        //public void ShowLegislatorsSponsoredBills(Legislator legislator)
        //{
        //    //SetLoadingUi();
        //
        //    //TODO RM:Make async task && FIX THIS GETTiNG CALLED EACH TIME ACTIVITY OR 
        //    //FRAGMENT IS RESTARTED (meaing a new call to server)
        //    //var bills = _billManager.GetBillsSponsoredbyLegislator(legislator.BioguideId, 1);
        //    _billAdapter.UpdateBill(bills);
        //
        //    SetLoadingUiOff();
        //}

        public void UpdateBills(List<Bill> bills)
        {
            _billAdapter.UpdateBill(bills);
            SetLoadingUiOff();
        }

        private void SetLoadingUiOff()
        {
            _emptyText.Text = AndroidHelper.GetString(Resource.String.emptyBillsText);
            ShowEmptyviewIfNecessary();
        }

        private void SetLoadingUi()
        {
            _emptyText.Text = AndroidHelper.GetString(Resource.String.loading);
            ShowEmptyview();
        }

        private void ShowEmptyview()
        {
            if (_viewSwitcher.NextView.Id == Resource.Id.billViewer_emptyText)
                _viewSwitcher.ShowNext();
        }

        private void ShowEmptyviewIfNecessary()
        {
            if (_billAdapter.ItemCount == 0 && _viewSwitcher.NextView.Id == Resource.Id.billViewer_emptyText)
                _viewSwitcher.ShowNext();
            else if (_billAdapter.ItemCount > 0 && _viewSwitcher.CurrentView.Id != Resource.Id.billViewer_recycler)
                _viewSwitcher.ShowNext();
        }
    }
}