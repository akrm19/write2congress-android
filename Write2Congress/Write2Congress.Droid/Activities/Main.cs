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
using Write2Congress.Droid.Interfaces;

namespace Write2Congress.Droid.Activities
{
    [Activity(MainLauncher =true)]
    public class Main : ToolBarSearchActivity
    {
        private MainFragment _mainFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.actv_Main);

            using (var toolbar = FindViewById<Toolbar>(Resource.Id.main_toolbar))
            {
                //SetActionBar(toolbar);
                SetSupportActionBar(toolbar);
                toolbar.Elevation = 10f;
            }

            //var actionMenu = FindViewById<Toolbar>(Resource.Id.main_bottomMenu);
            //actionMenu.InflateMenu(Resource.Menu.menu_action);
            //actionMenu.MenuItemClick += ActionMenu_MenuItemClick;
            
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
    }
}