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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected void SetupNavigationMenu(int navigationViewid)
        {
            using (var navigationView = FindViewById<NavigationView>(navigationViewid))
                navigationView.NavigationItemSelected += NavigationItemSelected;
        }


        protected void NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            e.MenuItem.SetChecked(true);
            e.Handled = true;

            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.actionMenu_drafts:
                    OpenDrafts();
                    break;
                case Resource.Id.actionMenu_sent:
                    OpenSent();
                    break;
                case Resource.Id.actionMenu_search:
                    OpenLegislatorSearch();
                    //e.Handled = true;
                    break;
                case Resource.Id.actionMenu_writeNew:
                    OpenWriteNewLetter();
                    break;
                default:
                    break;
            }

            CurrentDrawerLayout.CloseDrawers();
        }

        private void OpenWriteNewLetter()
        {
            if (GetType() == typeof(WriteLetterActivity))
                return;

            var intent = new Intent(this, typeof(WriteLetterActivity));
            StartActivity(intent);
        }

        private void OpenLegislatorSearch()
        {
            if (GetType() == typeof(MainActivity))
                return;

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        protected virtual void OpenDrafts()
        {
            var intent = new Intent(this, typeof(ViewLettersActivity));
            intent.PutExtra(BundleType.ViewLettersFragType, ViewLettersFragmentType.Drafts);
            StartActivity(intent);
        }

        protected virtual void OpenSent()
        {
            var intent = new Intent(this, typeof(ViewLettersActivity));
            intent.PutExtra(BundleType.ViewLettersFragType, ViewLettersFragmentType.Sent);
            StartActivity(intent);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_settings:
                    SettingsPressed();
                    return true;
                case Resource.Id.mainMenu_donate:
                    DonatePressed();
                    return true;
                case Resource.Id.mainMenu_exit:
                    ExitButtonPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void ExitButtonPressed()
        {
            FinishAffinity();
        }

        public void DonatePressed()
        {

        }

        public void SettingsPressed()
        {

        }
    }
}