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
using Android.Preferences;
using Android.Support.V7.Preferences;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public SettingsFragment() { }

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            AddPreferencesFromResource(Resource.Xml.preferences);
        }

        public override void OnStart()
        {
            base.OnStart();

            (Activity as Activities.BaseActivity).UpdateTitleBarText(AndroidHelper.GetString(Resource.String.settings));
        }
    }
}