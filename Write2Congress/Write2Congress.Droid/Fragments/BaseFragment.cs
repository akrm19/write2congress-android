using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.Activities;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid.Fragments
{
    public class BaseFragment : Android.Support.V4.App.Fragment
    {
        protected Logger MyLogger;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MyLogger = new Logger(Class.SimpleName);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        protected Toolbar SetupToolbar(View fragment, int toolbarResourceId, string title = "")
        {
            //This is needed, others Toolbar will not show
            HasOptionsMenu = true;

            var toolbar = fragment.FindViewById<Toolbar>(toolbarResourceId);
            
            toolbar.Elevation = 10f;
            toolbar.Title = string.IsNullOrWhiteSpace(title)
                ? null
                : title;

            GetBaseActivity().SetSupportActionBar(toolbar);

            return toolbar; 
        }

        public void ShowToast(string message, ToastLength lenght = ToastLength.Short)
        {
            Toast.MakeText(this.Context, message, lenght).Show();
        }

        protected void ExitButtonPressed()
        {
            (Activity as BaseToolbarActivity).ExitButtonPressed();
        }

        protected void DonatePressed()
        {
            (Activity as BaseToolbarActivity).DonatePressed();
        }

        protected void SettingsPressed()
        {
            (Activity as BaseToolbarActivity).SettingsPressed();
        }

        #region Helpers - Getter

        public BaseApplication GetBaseApp()
        {
            return Activity.Application as BaseApplication;
        }

        public BaseActivity GetBaseActivity()
        {
            return Activity as BaseActivity;
        } 

        protected Android.Support.V7.App.ActionBar GetToolbar()
        {
            return GetBaseActivity().SupportActionBar;
        }

        public List<Legislator> GetCachedLegislators()
        {
            //var legislators = (Activity.Application as BaseApplication).GetCachedLegislators();
            return AppHelper.GetCachedLegislators();
        }

        public LetterManager GetLetterManager()
        {
            return GetBaseApp().LetterManager;
        } 
        #endregion
    }
}