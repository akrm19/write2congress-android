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
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class ViewLegislatorActivity : BaseToolbarActivityWithButtons
    {
        private int _currentMenuId = Resource.Menu.menu_viewLegislator_favButtonOff;
        private ViewLegislatorFragment _viewLegislatorFragment;

        protected override int DrawerLayoutId => Resource.Id.viewLegislatorActv_parent;

        protected override int MenuItemId => _currentMenuId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actv_ViewLegislator);

            SetupToolbar(Resource.Id.viewLegislatorActv_toolbar);
            SetupNavigationMenu(Resource.Id.viewLegislatorActv_navigationDrawer);

            _viewLegislatorFragment = SupportFragmentManager.FindFragmentByTag(TagsType.ViewLegislatorsFragment) as ViewLegislatorFragment;

            if(_viewLegislatorFragment == null)
            {
                var serializedLegislator = AndroidHelper.GetStringFromIntent(Intent, BundleType.Legislator); 

                _viewLegislatorFragment = new ViewLegislatorFragment();

                //TODO RM (Low Priority):
                //http://stackoverflow.com/questions/9245408/best-practice-for-instantiating-a-new-android-fragment

                if(_viewLegislatorFragment.Arguments == null)
                    _viewLegislatorFragment.Arguments = new Bundle();

                _viewLegislatorFragment.Arguments.PutString(BundleType.Legislator, serializedLegislator);

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _viewLegislatorFragment, Resource.Id.viewLegislatorActv_fragmentContainer, TagsType.ViewLegislatorsFragment);
            }

            var adView = FindViewById<Android.Gms.Ads.AdView>(Resource.Id.viewLegislatorActv_adView);
            var adRequest = new Android.Gms.Ads.AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (AppHelper.IsLegislatorInFavorites(_viewLegislatorFragment.GetLegislator()))
            {
                _currentMenuId = Resource.Menu.menu_viewLegislator_favButtonOn;
            }
            else
                _currentMenuId = Resource.Menu.menu_viewLegislator_favButtonOff; 

            ReloadMenu();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Resource.Id.viewLegislator_menu_favButtonOff:
                    AppHelper.AddLegislatorToFavoriteList(_viewLegislatorFragment.GetLegislator());
                    ShowToast(AndroidHelper.GetString(Resource.String.legislatorAddedToFavorites));
                    _currentMenuId = Resource.Menu.menu_viewLegislator_favButtonOn;
                    break;
                case Resource.Id.viewLegislator_menu_favButtonOn:
                    AppHelper.RemoveLegislatorFromFavoriteList(_viewLegislatorFragment.GetLegislator());
                    ShowToast(AndroidHelper.GetString(Resource.String.legislatorRemovedFromFavorites));
                    _currentMenuId = Resource.Menu.menu_viewLegislator_favButtonOff;
                    break;
            }

            //ReloadMenu();
            SupportInvalidateOptionsMenu();
            return true;
        }
    }
}