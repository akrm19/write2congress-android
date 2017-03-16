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

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public abstract class BaseActivity : AppCompatActivity 
    {
        protected Logger Logger; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Logger = new Logger(Class.SimpleName);
        }

        protected void SetupToolbar(int toolbarResourceId, string title = "")
        {
            //var actionMenu = FindViewById<Toolbar>(Resource.Id.main_bottomMenu);
            //actionMenu.InflateMenu(Resource.Menu.menu_action);
            //actionMenu.MenuItemClick += ActionMenu_MenuItemClick;

            using (var toolbar = FindViewById<Toolbar>(toolbarResourceId))
            {
                //Unlike other attributes the toolbar title needs to be 
                //set first, otherwise app will default to activity tile
                toolbar.Title = string.IsNullOrWhiteSpace(title)
                    ? null
                    : title;

                SetSupportActionBar(toolbar);
                toolbar.Elevation = 10f;
            }
        }
    }
}