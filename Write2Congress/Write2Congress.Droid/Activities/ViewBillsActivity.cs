﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Enums;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "ViewBillsActivity")]
    public class ViewBillsActivity : BaseToolbarActivityWithSearch
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
        
            if(_viewBillsFragCtrl == null)
            {
                var serializedLegislator = AndroidHelper.GetStringFromIntent(Intent, BundleType.Legislator);

                //TODO RM: Finish implementing BillViewerFragmentCtrl for AllBillsOfEveryone
                _viewBillsFragCtrl = BillViewerFragmentCtrl.CreateInstance(BillViewerKind.AllBillsOfEveryone);

                if (_viewBillsFragCtrl.Arguments == null)
                    _viewBillsFragCtrl.Arguments = new Bundle();

                _viewBillsFragCtrl.Arguments.PutString(BundleType.Legislator, serializedLegislator);

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _viewBillsFragCtrl, Resource.Id.viewBillsActv_fragmentContainer, TagsType.ViewBillsFragment);                
            }
        }

        protected override int MenuItemId => Resource.Menu.menu_viewBills;

        protected override int SearchItemId => Resource.Id.viewBills_search;
    }
}