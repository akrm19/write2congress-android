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

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public abstract class BaseToolbarActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            SetupOnCreateOptionsMenu(menu);

            return base.OnCreateOptionsMenu(menu);
        }

        protected abstract void SetupOnCreateOptionsMenu(IMenu menu);

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_writeNew:
                    AppHelper.GetWriteNewLetterIntent(this);
                    return true;
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
                    return true;
            }
            //return base.OnOptionsItemSelected(item);
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

        protected void NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.actionMenu_drafts:
                    e.Handled = true;
                    OpenDrafts();
                    break;
                case Resource.Id.actionMenu_sent:
                    //e.Handled = true;
                    OpenSent();
                    break;
                default:
                    break;
            }
            //e.Handled = true;
        }

        private void OpenDrafts()
        {
            var viewDraftsFragment = new DraftLettersFragment();
            var containerId = Resource.Id.writeLetterActv_fragmentContainer;
            RepalceFragmentByTag(this, viewDraftsFragment, containerId, TagsType.WriteLetterFragment);
        }

        private void OpenSent()
        {
            var intent = new Intent(this, typeof(ViewLettersActivity));
            StartActivity(Intent);
        }

        private void RepalceFragmentByTag(BaseActivity activity, BaseFragment newFragment, int containerId, string tag)
        {
            //var transacton = activity.SupportFragmentManager.BeginTransaction();
            var transacton = activity.SupportFragmentManager.BeginTransaction();
            transacton.Replace(containerId, newFragment, TagsType.WriteLetterFragment);
            //transacton.AddToBackStack(null);
            transacton.Commit();
        }


    }


}