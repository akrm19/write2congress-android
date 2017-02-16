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
using Write2Congress.Droid.Helpers;

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
            SetActionBar(toolbar);
            toolbar.Elevation = 10f;
            //toolbar.InflateMenu(Resource.Menu.menu_main);
            //toolbar.MenuItemClick += Toolbar_MenuItemClick;
            
            
            _mainFragment = FragmentManager.FindFragmentByTag<MainFragment>(TagsType.MainParentFragment);

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddFragment(FragmentManager, _mainFragment, Resource.Id.main_fragmentContainer, TagsType.MainParentFragment);
            }
        }

        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.mainMenu_donate:
                    Toast.MakeText(ApplicationContext, "doonate", ToastLength.Short).Show();
                    break;
            }
        }
        /*
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_donate:
                    Toast.MakeText(ApplicationContext, "doonate", ToastLength.Short);
                    return true;

            }

            return base.OnOptionsItemSelected(item);
        }

        */

    }
}