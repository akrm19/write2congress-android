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

namespace Write2Congress.Droid.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //
        //    //Add preferences from XML resource
        //    AddPreferencesFromResource(Resource.Xml.preferences);
        //}

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            AddPreferencesFromResource(Resource.Xml.preferences);
        }

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    // Use this to return your custom view for this Fragment
        //    // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
        //
        //    return base.OnCreateView(inflater, container, savedInstanceState);
        //}
    }
}