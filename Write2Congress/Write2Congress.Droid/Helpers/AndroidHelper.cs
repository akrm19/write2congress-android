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

namespace Write2Congress.Droid.Helpers
{
    public class AndroidHelper
    {
        public static void AddFragment(FragmentManager fragmentManager, Fragment fragment, int containerId, string tag)
        {
            var transaction = fragmentManager.BeginTransaction();

            transaction.Add(containerId, fragment, tag);
            transaction.Commit();
        }

    }
}