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
using Write2Congress.Shared.BusinessLayer;
using Newtonsoft.Json;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Code;
using Android.Support.V4.View;
using SearchView = Android.Support.V7.Widget.SearchView;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Write2Congress.Droid.DomainModel.Enums;
using Android.Support.V7.App;
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.Activities
{
    [Activity(MainLauncher =true)]
    public class MainActivity : BaseToolbarActivityWithSearch, IActivityWithToolbarSearch
    {
        private MainFragment _mainFragment;

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.mainActv_parent;
            }
        }

        protected override int DrawerMenuItemId => Resource.Id.actionMenu_search;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.actv_Main);

            SetupToolbar(Resource.Id.mainActv_toolbar);
            SetupNavigationMenu(Resource.Id.mainActv_navigationDrawer);

            var id = "ca-app-pub-2083619170491780~6859816114";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);

            _mainFragment = SupportFragmentManager.FindFragmentByTag(TagsType.MainParentFragment) as MainFragment;

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _mainFragment, Resource.Id.mainActv_fragmentContainer, TagsType.MainParentFragment);
            }

            var adView = FindViewById<Android.Gms.Ads.AdView>(Resource.Id.adView2);
#if DEBUG
            adView.AdUnitId = Resources.GetString(Resource.String.banner_ad_unit_id_TEST);
#else
            adView.AdUnitId = Resources.GetString(Resource.String.banner_ad_unit_id);
#endif
            var adRequest = new Android.Gms.Ads.AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_refresh:
                    UpdateLegislatorsWithPrompt();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void UpdateLegislatorsWithPrompt()
        {
            if (!LegislatorsUpdatedInLast30Days())
                UpdateLegislators();
            else
                VerifyUserWantsToUpdateLegislators();
        }

        private void UpdateLegislators()
        {
            var results = GetBaseApp().UpdateLegislatorData();
            var message = AndroidHelper.GetString(results
                ? Resource.String.legislatorDataSuccessfullyUpdated
                : Resource.String.unableToUpdateLegislatorData);

            ShowToast(message);
        }

        private bool LegislatorsUpdatedInLast30Days()
        {
            var lastUpdate = AppHelper.GetLastLegislatorUpdate();

            return lastUpdate == DateTime.MinValue
                || lastUpdate.CompareTo(DateTime.Now.AddDays(-30)) >= 0;
        }

        private void VerifyUserWantsToUpdateLegislators()
        {
            var lastUpdate = AppHelper.GetLastLegislatorUpdate();

            var message = string.Format("{0}{1}{1}{2}{1}{1}{3}",
                AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorData),
                System.Environment.NewLine,
                AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorDataWarning),
                lastUpdate == DateTime.MinValue
                    ? string.Empty
                    : $"{AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorDataLastUpdate)}: {lastUpdate.ToString("G")}");

            var verifyPrompt = new Android.Support.V7.App.AlertDialog.Builder(this);//, Resource.Style.VerifyDialogTheme);
            verifyPrompt.SetTitle(AndroidHelper.GetString(Resource.String.confirmRefresh));
            verifyPrompt.SetMessage(message);
            verifyPrompt.SetNegativeButton(Resource.String.dismiss,
                (sender, args) => 
                    {
                        RunOnUiThread(() => (sender as Android.Support.V7.App.AlertDialog).Dismiss());
                    });
            verifyPrompt.SetPositiveButton(Resource.String.ok,
                (sender, args) =>
                {
                    RunOnUiThread(UpdateLegislators);
                });

            verifyPrompt.Create().Show();
        }

        protected override int MenuItemId
        {
            get
            {
                return Resource.Menu.menu_main;
            }
        }

        protected override int FilterDataItemId
        {
            get
            {
                return Resource.Id.mainMenu_search;
            }
        }
    }
}