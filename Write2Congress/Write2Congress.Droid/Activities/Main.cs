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
            SetupToolbar(Resource.Id.mainActv_toolbar);
            
            _mainFragment = FragmentManager.FindFragmentByTag<MainFragment>(TagsType.MainParentFragment);

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddFragment(FragmentManager, _mainFragment, Resource.Id.mainActv_fragmentContainer, TagsType.MainParentFragment);
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