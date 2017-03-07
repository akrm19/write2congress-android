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
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Fragments
{
    public class BaseFragment : Fragment
    {
        public Logger Logger2;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //TODO RM: Rename this to Logger and Logger to MyLogger
            Logger2 = new Logger(Class.SimpleName);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }


        protected BaseApplication GetBaseApp()
        {
            return Activity.Application as BaseApplication;
        }

        protected List<Legislator> GetCachedLegislators()
        {
            //var legislators = (Activity.Application as BaseApplication).GetCachedLegislators();
            return AppHelper.GetCachedLegislators();
        }
    }
}