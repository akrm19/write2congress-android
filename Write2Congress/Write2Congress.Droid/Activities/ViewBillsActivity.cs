
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
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "ViewBillsActivity")]
    public class ViewBillsActivity : BaseToolbarActivity
    {
        private BillViewerFragmentCtrl _viewBillsFragCtrl;

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.viewBillsActv_parent;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actv_ViewBills);

            SetupToolbar(Resource.Id.viewBillsActv_toolbar);
            SetupNavigationMenu(Resource.Id.viewLBillsActv_navigationDrawer);

            _viewBillsFragCtrl = SupportFragmentManager.FindFragmentByTag(TagsType.SponsoredBillsFragment) as BillViewerFragmentCtrl;
        /*
            if(_viewBillsFragCtrl == null)
            {
                var serializedLegislator = AndroidHelper.GetStringFromIntent(Intent, BundleType.Legislator);

                _viewBillsFragCtrl = new BillViewerFragmentCtrl();

                if (_viewBillsFragCtrl.Arguments == null)
                    _viewBillsFragCtrl.Arguments = new Bundle();

                _viewBillsFragCtrl.Arguments.PutString(BundleType.Legislator, serializedLegislator);

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _viewBillsFragCtrl, Resource.Id.viewBillsActv_fragmentContainer, TagsType.ViewBillsFragment);                
            }
            */
        }
    }
}
