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
using Write2Congress.Droid.Code;
using Android.Support.V7.Widget;
using Write2Congress.Droid.Adapters;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid.CustomControls
{
    public class LegislatorsViewer : LinearLayout
    {
        Logger Logger;
        RecyclerView _recyclerView;
        RecyclerView.LayoutManager _layoutManager;
        LegislatorAdapter _legislatorAdapter;

        Spinner _states;
        //Address _currentAddress;
        StateOrTerritory _defaultStateOrTerritory = StateOrTerritory.ALL;

        List<string> _stateNames;
        List<Legislator> _legislators;

        BaseFragment _fragment;


        public LegislatorsViewer(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public LegislatorsViewer(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            Logger = new Logger(Class.SimpleName);

            using (var layoutInflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater)
                layoutInflater.Inflate(Resource.Layout.ctrl_LegislatorsViewer, this, true);
        }

        public void FilterLegislatorsByFirstMiddleOrLastName(string filter)
        {
            _legislatorAdapter.UpdateLegislators(_legislators.FilterByFirstMiddleOrLastName(filter));
        }

        public void FilterByStateOrTerritory(StateOrTerritory stateOrterritory)
        {
            SetStateSpinner(stateOrterritory);
        }

        public void SetupCtrl(BaseFragment fragment, List<Legislator> legislators)
        {
            _fragment = fragment;
            //Setup Legislator RecyclerView 
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.legislatorsViewer_legislatorsRecycler);
            _layoutManager = new LinearLayoutManager(_fragment.Context, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);
            //Setup Legislator Adapater
            _legislators = AppHelper.GetCachedLegislators();
            _legislatorAdapter = new LegislatorAdapter(_fragment, _legislators);
            _recyclerView.SetAdapter(_legislatorAdapter);

            //Setup States spinner
            _states = FindViewById<Spinner>(Resource.Id.legislatorsViewer_statesSpinner);
            _stateNames = StateOrTerritory.GetNames(typeof(StateOrTerritory)).ToList();
            var statesAdapter = new ArrayAdapter<string>(_fragment.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, _stateNames);
            _states.Adapter = statesAdapter;
            _states.ItemSelected += _states_ItemSelected;

        }

        private StateOrTerritory ParseFromString(string stateOrTerritoryString, StateOrTerritory defaultStateOrTerritory)
        {
            StateOrTerritory stateOrTerritory;
            
            if (Enum.TryParse<StateOrTerritory>(stateOrTerritoryString, out stateOrTerritory))
                return stateOrTerritory;

            Logger.Error($"Could not parse StateOrTerritory: {stateOrTerritoryString}. Returning default value {defaultStateOrTerritory}");
            return defaultStateOrTerritory;
        }


        private void SetStateSpinner(StateOrTerritory stateOrTerritory)
        {
            var position = GetStateOrTerritoryPosition(_stateNames, stateOrTerritory);
            if (position >= 0)
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
            StateOrTerritory stateOrTerritory = StateOrTerritory.ALL;

            if (!Enum.TryParse<StateOrTerritory>(selectedState, out stateOrTerritory))
            {

                Logger.Error($"Could not parse select State: {selectedState}");
                return;
            }

            try
            {
                if (_legislatorAdapter == null)
                {
                    Logger.Error($"Legislator adapater is null. Cannot select legislators for state ({selectedState}).");
                    return;
                }

                _fragment.Activity.RunOnUiThread(() =>
                {
                  _legislatorAdapter.UpdateLegislators(_legislators.FilterByState(stateOrTerritory));
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Cannot select legislators for selected State ({selectedState}). Error: {ex.Message}");
            }
        }
    }
}