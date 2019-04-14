
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
using Write2Congress.Droid.DomainModel.Interfaces;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public abstract class ViewBillsBaseActivity : BaseToolbarActivityWithSearch
    {
        protected virtual BillViewerKind GetBillViewerKind { get; }

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

            var _viewBillsFragCtrl = SupportFragmentManager.FindFragmentByTag(TagsType.ViewBillsFragment) as BillViewerFragmentCtrl;

            if (_viewBillsFragCtrl == null)
            {
                _viewBillsFragCtrl = BillViewerFragmentCtrl.CreateInstance(GetBillViewerKind);
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _viewBillsFragCtrl, Resource.Id.viewBillsActv_fragmentContainer, TagsType.ViewBillsFragment);
            }

            var adView = FindViewById<Android.Gms.Ads.AdView>(Resource.Id.viewBillsActv_adView2);
#if DEBUG
            adView.AdUnitId = Resources.GetString(Resource.String.banner_ad_unit_id_TEST);
#else
            adView.AdUnitId = Resources.GetString(Resource.String.banner_ad_unit_id);
#endif
            var adRequest = new Android.Gms.Ads.AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
        }
    }
}
