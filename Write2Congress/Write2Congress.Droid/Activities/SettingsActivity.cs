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
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class SettingsActivity : BaseToolbarActivity
    {
        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.settingsActv_parent;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actv_Settings);

            SetupToolbar(Resource.Id.settingsActv_toolbar, AndroidHelper.GetString(Resource.String.settings));
            SetupNavigationMenu(Resource.Id.settingsActv_navigationDrawer);


            
        }
    }
}