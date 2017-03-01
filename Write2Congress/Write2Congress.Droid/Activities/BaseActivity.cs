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

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "Write2Congress")]
    public class BaseActivity : Activity
    {
        protected Logger Logger; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Logger = new Logger(Class.SimpleName);
        }
    }
}