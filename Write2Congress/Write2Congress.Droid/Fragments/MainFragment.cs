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
            //Inflate fragment
            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);

            _legislatorsViewer = mainFragment.FindViewById<LegislatorsViewer>(Resource.Id.mainFrag_legislatorsViewer);
            _legislatorsViewer.SetupCtrl(this, AppHelper.GetCachedLegislators());
            
            //Temp
            using (var button = mainFragment.FindViewById<Button>(Resource.Id.mainFrag_myButton))
                button.Click += delegate {
                    var searchInputTest = mainFragment.FindViewById<EditText>(Resource.Id.mainFrag_zip);
                    _legislatorsViewer.FilterLegislatorsByFirstMiddleOrLastName(searchInputTest.Text);
                };

            return mainFragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _currentAddress = GeoHelper.GetCurrentAddress();
            StateOrTerritory state = StateOrTerritory.ALL;

            if (GeoHelper.IsAddressInUs(_currentAddress))
            {
                //TODO RM: _currentAddress.AdminArea returns full state name, like Texas, need to handle 
                //this since it is imcopatible w/current enum
                
                state = ParseFromString(_currentAddress.AdminArea, state);

                _legislatorsViewer.FilterByStateOrTerritory(state);
            }

            Logger2.Info("User is not in US (use is in {0}). Setting selected state to: {1}",
                _currentAddress.CountryName ?? string.Empty,
                state.ToString());

            _legislatorsViewer.FilterByStateOrTerritory(state);

            var searchInputTest = View.FindViewById<EditText>(Resource.Id.mainFrag_zip);
            searchInputTest.Text = _currentAddress.PostalCode;
        }

        private StateOrTerritory ParseFromString(string stateOrTerritoryString, StateOrTerritory defaultStateOrTerritory)
        {
            StateOrTerritory stateOrTerritory;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerritoryString, out stateOrTerritory))
                return stateOrTerritory;

            Logger2.Error($"Could not parse StateOrTerritory: {stateOrTerritoryString}. Returning default value {defaultStateOrTerritory}");
            return defaultStateOrTerritory;
        }
    }
}