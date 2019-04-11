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
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.Code;
using Android.Support.Design.Widget;
using Write2Congress.Droid.Fragments;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public abstract class BaseToolbarActivity : BaseActivity
    {
        private IMenuItem previousMenuItem = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected void SetupNavigationMenu(int navigationViewid)
        {

            using (var navigationView = FindViewById<NavigationView>(navigationViewid))
            {
                if (previousMenuItem != null)
                    previousMenuItem.SetChecked(false);

                previousMenuItem = navigationView.Menu.FindItem(DrawerMenuItemId);
                previousMenuItem.SetChecked(true);
                navigationView.NavigationItemSelected += NavigationItemSelected;
            }
        }

        protected void NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //e.MenuItem.SetChecked(true);
            e.Handled = true;

            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.actionMenu_search:
                    OpenLegislatorSearch();
                    //e.Handled = true;
                    break;
                case Resource.Id.actionMenu_settings:
                    SettingsPressed();
                    break;
                case Resource.Id.actionMenu_feedback:
                    FeedbackPressed();
                    break;
                case Resource.Id.actionMenu_donate:
                    DonatePressed();
                    break;
                case Resource.Id.actionMenu_exit:
                    ExitButtonPressed();
                    break;
                case Resource.Id.actionMenu_searchBills:
                    OpenBillsSearch();
                    break;
                case Resource.Id.actionMenu_latestBills:
                    OpenLatestBills();
                    break;
                case Resource.Id.actionMenu_favoriteLegislators:
                    OpenFavLegislators();
                    break;
                default:
                    break;
            }

            CurrentDrawerLayout.CloseDrawers();
        }

        private void FeedbackPressed()
        {
            if (GetType() == typeof(FeedbackActivity))
                return;

            var intent = new Intent(this, typeof(FeedbackActivity));
            StartActivity(intent);
        }

        private void OpenFavLegislators()
        {
            if (GetType() == typeof(FavoriteLegislatorsActivity))
                return;

            var intent = new Intent(this, typeof(FavoriteLegislatorsActivity));
            StartActivity(intent);
        }

        private void OpenLatestBills()
        {
            if (GetType() == typeof(ViewLastestBillsActivity))
                return;

            var intent = new Intent(this, typeof(ViewLastestBillsActivity));
            StartActivity(intent);
        }

        private void OpenBillsSearch()
        {
            if (GetType() == typeof(SearchBillsActivity))
                return;

            var intent = new Intent(this, typeof(SearchBillsActivity));
            StartActivity(intent);
        }

        private void OpenLegislatorSearch()
        {
            if (GetType() == typeof(MainActivity))
                return;

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        public void ExitButtonPressed()
        {
            FinishAffinity();
        }

        public void DonatePressed()
        {
            if (GetType() == typeof(DonateActivity))
                return;

            var intent = new Intent(this, typeof(DonateActivity));
            StartActivity(intent);
        }

        public void SettingsPressed()
        {
            if (GetType() == typeof(SettingsActivity))
                return;

            var intent = new Intent(this, typeof(SettingsActivity));
            StartActivity(intent);
        }
    }
}