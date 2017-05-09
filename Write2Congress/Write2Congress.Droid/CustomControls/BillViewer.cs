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
    public class BillViewer : BaseViewer
    {
        private BillManager _billManager;
        private List<Bill> _bills;

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

        public override void SetupCtrl(BaseFragment fragment)
        {
            base.SetupCtrl(fragment);

            _billManager = new BillManager(myLogger);

            recyclerAdapter = new BillAdapter(fragment);
            recycler.SetAdapter(recyclerAdapter);

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
            (recyclerAdapter as BillAdapter).UpdateBill(bills);
            SetLoadingUiOff();
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyBillsText);
        }

        protected override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.billsSponsored);
        }
    }
}