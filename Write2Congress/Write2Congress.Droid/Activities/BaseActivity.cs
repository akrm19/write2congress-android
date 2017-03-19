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
        protected Logger MyLogger; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             
            MyLogger = new Logger(Class.SimpleName);
        }

        #region Helpers
        protected void ReplaceFragmentByTag(BaseActivity activity, BaseFragment newFragment, int containerId, string tag)
        {
            var transacton = activity.SupportFragmentManager.BeginTransaction();
            transacton.Replace(containerId, newFragment, tag);
            transacton.Commit();
        }
        #endregion
    }
}