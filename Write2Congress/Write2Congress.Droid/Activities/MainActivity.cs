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
using Write2Congress.Droid.Interfaces;
using Android.Support.Design.Widget;

namespace Write2Congress.Droid.Activities
{
    [Activity(MainLauncher =true)]
    public class MainActivity : BaseToolbarActivity, ILegislatorViewerActivity
    {
        private MainFragment _mainFragment;
        //New listener
        private SearchTextChangedDelegate _legislatorSearchTextChanged;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.actv_Main);
            SetupToolbar(Resource.Id.mainActv_toolbar);

            SetupNavigationMenu(Resource.Id.mainActv_navigationDrawer);

            _mainFragment = SupportFragmentManager.FindFragmentByTag(TagsType.MainParentFragment) as MainFragment;

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _mainFragment, Resource.Id.mainActv_fragmentContainer, TagsType.MainParentFragment);
            }
        }

        protected void SetupToolbar(int toolbarResourceId, string title = "")
        {
            using (var toolbar = FindViewById<Toolbar>(toolbarResourceId))
            {
                //Unlike other attributes the toolbar title needs to be 
                //set first, otherwise app will default to activity tile
                SetSupportActionBar(toolbar);

                if (string.IsNullOrWhiteSpace(title))
                    SupportActionBar.SetDisplayShowTitleEnabled(false);
                else
                    toolbar.Title = title;

                toolbar.Elevation = 10f;
            }
        }

        private void ActionMenu_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.actionMenu_search:
                    Toast.MakeText(ApplicationContext, "search", ToastLength.Short).Show();
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_writeNew:
                    AppHelper.StartWriteNewLetterIntent(this);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            using (var searchMenuitem = menu.FindItem(Resource.Id.mainMenu_search))
            using (var searchView = MenuItemCompat.GetActionView(searchMenuitem))
            using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                searchViewJavaObj.QueryTextChange += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.NewText);
                };

                searchViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.Query);
                    e.Handled = true;
                };
            }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            _legislatorSearchTextChanged = null;

            base.OnDestroy();
        }

        public SearchTextChangedDelegate LegislatorSearchTextChanged
        {
            get
            {
                return _legislatorSearchTextChanged;
            }
            set
            {
                _legislatorSearchTextChanged += value;
            }
        }

        public void ClearLegislatorSearchTextChangedDelegate()
        {
            _legislatorSearchTextChanged = null;
        }
    }
}