
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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;
using Write2Congress.Droid.CustomControls;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Fragments
{
    public class FavoriteLegislatorsFragment : BaseFragment
    {
        LegislatorsViewer _legislatorsViewer;

        public FavoriteLegislatorsFragment() {}

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;


            //Inflate fragment
            var fragment = inflater.Inflate(Resource.Layout.frag_FavoriteLegislators, container, false);

            //Setup legislatorsViewer
            _legislatorsViewer = fragment.FindViewById<LegislatorsViewer>(Resource.Id.favortieLegislatorsFrag_legislatorsViewer);
            _legislatorsViewer.SetupCtrl(this, AppHelper.GetFavoriteLegislators(), false);

            return fragment;
        }
    }
}
