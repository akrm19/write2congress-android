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

namespace Write2Congress.Droid.Fragments
{
    public class MainFragment : BaseFragment
    {
        RecyclerView _recyclerView;
        RecyclerView.LayoutManager _layoutManager;
        LegislatorAdapter _legislatorAdapter;

        Spinner _states;
        Address _currentAddress;
        StateOrTerritory _defaultStateOrTerritory = StateOrTerritory.ALL;

        List<string> _stateNames;
        List<Legislator> _legislators;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Inflate fragment
            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);

            //Setup Legislator RecyclerView 
            _recyclerView = mainFragment.FindViewById<RecyclerView>(Resource.Id.mainFrag_legislatorsParentRecycler);
            _layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);
            //Setup Legislator Adapater
            _legislators = AppHelper.GetCachedLegislators();
            _legislatorAdapter = new LegislatorAdapter(this, _legislators);
            _recyclerView.SetAdapter(_legislatorAdapter);

            //Setup States spinner
            _states = mainFragment.FindViewById<Spinner>(Resource.Id.mainFrag_states);
            _stateNames = Enum.GetNames(typeof(StateOrTerritory)).ToList();
            var statesAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, _stateNames);
            _states.Adapter = statesAdapter;
            _states.ItemSelected += _states_ItemSelected;

            //Temp
            using (var button = mainFragment.FindViewById<Button>(Resource.Id.mainFrag_myButton))
                button.Click += delegate {
                    var searchInputTest = mainFragment.FindViewById<EditText>(Resource.Id.mainFrag_zip);
                    _legislatorAdapter.UpdateLegislators(_legislators.FilterByFirstMiddleOrLastName(searchInputTest.Text));
                    //_legislatorAdapter.NotifyDataSetChanged();
                };

            return mainFragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _currentAddress = GeoHelper.GetCurrentAddress();

            if (GeoHelper.IsAddressInUs(_currentAddress))
            {
                //TODO RM: _currentAddress.AdminArea returns full state name, like Texas, need to handle 
                //this since it is imcopatible w/current enum
                
                var state = _currentAddress.AdminArea.ToLower().Equals("texas")
                    ? StateOrTerritory.TX
                    : ParseFromString(_currentAddress.AdminArea, _defaultStateOrTerritory);

                SetStateSpinner(state);
            }

            Logger2.Info("User is not in US (use is in {0}). Setting selected state to: {1}",
                _currentAddress.CountryName ?? string.Empty,
                _defaultStateOrTerritory.ToString());

            SetStateSpinner(_defaultStateOrTerritory);

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

        private void SetStateSpinner(StateOrTerritory stateOrTerritory)
        {
            var position = GetStateOrTerritoryPosition(_stateNames, stateOrTerritory);
            if(position >= 0)
                _states.SetSelection(position);
        }

        //TODO RM: Change to generic extension
        private int GetStateOrTerritoryPosition(List<string> stateOrTerritories, StateOrTerritory lookupItem)
        {
            var stringVal = lookupItem.ToString();
            return stateOrTerritories.FindIndex(s => s.Equals(lookupItem));
        }

        private void _states_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            var selectedState = Convert.ToString(spinner.GetItemAtPosition(e.Position));
            StateOrTerritory stateOrTerritory;

            if(!Enum.TryParse<StateOrTerritory>(selectedState, out stateOrTerritory))
            {
            
                Logger2.Error($"Could not parse select State: {selectedState}");
                return;
            }

            try
            {
                if (_legislatorAdapter == null)
                {
                    Logger2.Error($"Legislator adapater is null. Cannot select legislators for state ({selectedState}).");
                    return;
                }

                this.Activity.RunOnUiThread(() =>
                {
                    _legislatorAdapter.UpdateLegislators(_legislators.FilterByState(stateOrTerritory));
                });
            }
            catch (Exception ex)
            {
                Logger2.Error($"Cannot select legislators for selected State ({selectedState}). Error: {ex.Message}");
            }
        }
    }
}