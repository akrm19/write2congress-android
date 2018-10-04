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
using Android.Support.V7.App;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.BusinessLayer;
using Android.Support.V4.Widget;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public abstract class BaseActivity : AppCompatActivity 
    {
        protected Logger MyLogger;
        protected abstract int DrawerLayoutId { get; }
        private DrawerLayout _currentDrawerLayout;
        private int _toolbarId;

        public DrawerLayout CurrentDrawerLayout
        {
            get
            {
                if (_currentDrawerLayout == null)
                    _currentDrawerLayout = FindViewById<DrawerLayout>(DrawerLayoutId);

                return _currentDrawerLayout;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             
            MyLogger = new Logger(Class.SimpleName);
        }

        public void SetupToolbar(int toolbarResourceId)
        {
            _toolbarId = toolbarResourceId;

            using (var toolbar = FindViewById<Toolbar>(toolbarResourceId))
            {
                //Unlike other attributes the toolbar title needs to be 
                //set first, otherwise app will default to activity tile
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);

                SupportActionBar.Elevation = 10f;
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_action_menu);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                var drawerToggle = new ActionBarDrawerToggle(this, CurrentDrawerLayout, toolbar, Resource.String.termStarted, Resource.String.termEnds);
                CurrentDrawerLayout.AddDrawerListener(drawerToggle);
                drawerToggle.SyncState();
            }
        }

        #region Helpers

        public Toolbar GetSupportToolbar()
        {
            return FindViewById<Android.Support.V7.Widget.Toolbar>(_toolbarId);
        }

        public void UpdateTitleBarText(string newText)
        {
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.Title = newText;                   
        }

        protected void ReplaceFragmentByTag(BaseActivity activity, BaseFragment newFragment, int containerId, string tag)
        {
            var transacton = activity.SupportFragmentManager.BeginTransaction();
            transacton.Replace(containerId, newFragment, tag);
            transacton.Commit();
        }

        public BaseApplication GetBaseApp()
        {
            return Application as BaseApplication;
        }

        public void ShowToast(string message, ToastLength lenght = ToastLength.Short)
        {
            Toast.MakeText(ApplicationContext, message, lenght).Show();
        }
        #endregion
    }
}