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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "Main", MainLauncher =true)]
    public class Main : BaseActivity
    {
        private MainFragment _mainFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.actv_Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar);
            //SetActionBar(toolbar);
            SetSupportActionBar(toolbar);
            toolbar.Elevation = 10f;

            var actionMenu = FindViewById<Toolbar>(Resource.Id.main_bottomMenu);
            actionMenu.InflateMenu(Resource.Menu.menu_action);
            actionMenu.MenuItemClick += ActionMenu_MenuItemClick;
            
            
            _mainFragment = FragmentManager.FindFragmentByTag<MainFragment>(TagsType.MainParentFragment);

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddFragment(FragmentManager, _mainFragment, Resource.Id.main_fragmentContainer, TagsType.MainParentFragment);
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            var searchMenuitem = menu.FindItem(Resource.Id.mainMenu_search);
            var searchView = MenuItemCompat.GetActionView(searchMenuitem);

            var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();
            searchViewJavaObj.QueryTextChange += (s, e) => Toast.MakeText(this, e.NewText, ToastLength.Short);
            searchViewJavaObj.QueryTextSubmit += (s, e) => Toast.MakeText(this, "Search Query Submitted: " + e.Query, ToastLength.Long);

            //MenuItemCompat.SetOnActionExpandListener(searchMenuitem, new Sea)

            return base.OnCreateOptionsMenu(menu);
        }

        private class SearchViewExpandListener : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
        {
            private readonly IFilterable _adapter;

            public SearchViewExpandListener(IFilterable adapter)
            {
                _adapter = adapter;
            }

            public bool OnMenuItemActionCollapse(IMenuItem item)
            {
                _adapter.Filter.InvokeFilter("");
                return true;
            }

            public bool OnMenuItemActionExpand(IMenuItem item)
            {
                return true;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_donate:
                    Toast.MakeText(ApplicationContext, "doonate", ToastLength.Short).Show();
                    return true;

            }

            return base.OnOptionsItemSelected(item);
        }
    }


    public class SearchViewExpandListener : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
    {
        private readonly IFilterable _adapter;

        public SearchViewExpandListener(IFilterable adapter)
        {
            _adapter = adapter;
        }

        public bool OnMenuItemActionCollapse(IMenuItem item)
        {
            _adapter.Filter.InvokeFilter("");
            return true;
        }

        public bool OnMenuItemActionExpand(IMenuItem item)
        {
            return true;
        }
    }
}