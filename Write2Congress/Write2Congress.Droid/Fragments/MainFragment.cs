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
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using Write2Congress.Droid.Adapters;
using Write2Congress.Droid.Code;
using Android.Locations;
using Android.Support.V4.App;
using static Android.Manifest;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Droid.CustomControls;

namespace Write2Congress.Droid.Fragments
{
    public class MainFragment : BaseFragment
    {
        LegislatorsViewer _legislatorsViewer;
        Address _currentAddress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //This is needed to tell host activity that the fragment as menu options to add
            HasOptionsMenu = true;

            //Inflate fragment
            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);

            //Setup legislatorsViewer
            _legislatorsViewer = mainFragment.FindViewById<LegislatorsViewer>(Resource.Id.mainFrag_legislatorsViewer);
            _legislatorsViewer.SetupCtrl(this, AppHelper.GetCachedLegislators());

            return mainFragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _currentAddress = GeoHelper.GetCurrentAddress();
            StateOrTerritory state = AppHelper.GetUsaStateFromAddress(_currentAddress);
            _legislatorsViewer.FilterByStateOrTerritory(state);
        }
    }
}