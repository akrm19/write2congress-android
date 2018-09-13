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
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class SettingsActivity : BaseToolbarActivity
    {
        private SettingsFragment _settingsFrag;

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

            SetupToolbar(Resource.Id.settingsActv_toolbar);
            SetupNavigationMenu(Resource.Id.settingsActv_navigationDrawer);

            if(_settingsFrag == null)
            {
                _settingsFrag = new SettingsFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _settingsFrag, Resource.Id.settingsActv_fragmentContainer, TagsType.SettingsFragment);
            }
        }
    }
}