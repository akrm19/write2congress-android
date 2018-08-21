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
using Write2Congress.Droid.DomainModel.Enums;
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.CustomControls
{
    public class LegislatorsViewer : LinearLayout
    {
        private LegislatorAdapter _legislatorAdapter;
        private Spinner _statesAndTerrSpinner;
        private BaseFragment _fragment;

        private List<string> _stateAndTerrNames;
        private List<Tuple<StateOrTerritory, string>> _statesAndTerrWithDescription;
        private List<Legislator> _legislators;

        protected Logger Logger;

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
            FilterLegislatorsByFirstMiddleOrLastName(filter, true);
        }

        public void FilterLegislatorsByFirstMiddleOrLastName(string filter, bool filterSelectedStateOrTerr = true)
        {
            if(filterSelectedStateOrTerr)
            {
                try
                {
                    StateOrTerritory selectedStateOrTerr =  _statesAndTerrWithDescription[_statesAndTerrSpinner.SelectedItemPosition].Item1;
                    _legislatorAdapter.UpdateLegislators(_legislators.FilterByState(selectedStateOrTerr).FilterByFirstMiddleOrLastName(filter));
                }
                catch (Exception ex)
                {
                    Logger.Error("Error encountered filtering Legislators by first, middle, or last name.", ex);
                }
            }
            else
                _legislatorAdapter.UpdateLegislators(_legislators.FilterByFirstMiddleOrLastName(filter));
        }
        
        public void FilterByStateOrTerritory(StateOrTerritory stateOrterritory)
        {
            SetStateSpinner(stateOrterritory);
        }

        public void SetupCtrl(BaseFragment fragment, List<Legislator> legislators, bool showStateSpinner = true)
        {
            _fragment = fragment;
            _legislators = legislators;
            
            //Setup Legislator RecyclerView 
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.legislatorsViewer_legislatorsRecycler);
            var layoutManager = new LinearLayoutManager(_fragment.Context, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);

            //Setup Legislator Adapater
            _legislatorAdapter = new LegislatorAdapter(_fragment, _legislators);
            _legislatorAdapter.WriteLetterToLegislatorClick += WriteNewLetterItemClicked;
            _legislatorAdapter.LegislatorClick += LegislatorClicked;
            recyclerView.SetAdapter(_legislatorAdapter);

			//Setup States spinner
			_statesAndTerrSpinner = FindViewById<Spinner>(Resource.Id.legislatorsViewer_statesSpinner);

            if (showStateSpinner)
            {
                _statesAndTerrWithDescription = Util.GetAllStatesAndTerrWithDescriptions();
                _stateAndTerrNames = _statesAndTerrWithDescription.Select(s => s.Item2).ToList();

                var statesAdapter = new ArrayAdapter<string>(_fragment.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, _stateAndTerrNames);
                _statesAndTerrSpinner.Adapter = statesAdapter;
                _statesAndTerrSpinner.ItemSelected += _states_ItemSelected;
            }
            else
                _statesAndTerrSpinner.Visibility = ViewStates.Gone;

            HookupToActivitySearchTextChangedDelegate();
        }

        private void LegislatorClicked(object sender, int position)
        {
            var legislator = _legislatorAdapter.GetLegislatorAtPosition(position);

            if (legislator == null)
            {
                Logger.Error("Error opening legislator details. Unable to retrive legislator at positition " + position);
                return;
            }

            AppHelper.StartViewLegislatorIntent(_fragment.GetBaseActivity(), legislator);
        }

        void WriteNewLetterItemClicked(object sender, int position)
        {
            var legislator = _legislatorAdapter.GetLegislatorAtPosition(position);

            if(legislator == null)
            {
                Logger.Error("Unable to write to legislator. Unable to retrive legislator at positition " + position);
                return;
            }

            AppHelper.StartWriteNewLetterIntent(_fragment.GetBaseActivity(), BundleSenderKind.LegislatorViewer, legislator);
        }

        private void HookupToActivitySearchTextChangedDelegate()
        {
            var par = _fragment.Activity as IActivityWithToolbarSearch;

            if(par != null)
                par.FilterSearchTextChanged += FilterLegislatorsByFirstMiddleOrLastName;
        }

        protected override void Dispose(bool disposing)
        {
            _legislatorAdapter = null;
            _statesAndTerrSpinner = null;
            _fragment = null;

            _stateAndTerrNames = null;
            _statesAndTerrWithDescription = null;
            _legislators = null;

            base.Dispose(disposing);
        }


        private void SetStateSpinner(StateOrTerritory stateOrTerritory)
        {
            var position = (int)stateOrTerritory;  //GetStateOrTerritoryPosition(_stateAndTerrNames, stateOrTerritory);

            if (position >= 0)
                _statesAndTerrSpinner.SetSelection(position);
        }

        private int GetStateOrTerritoryPosition(List<string> stateOrTerritories, StateOrTerritory lookupItem)
        {
            var stringVal = _statesAndTerrWithDescription[(int)lookupItem].Item2;

            return stateOrTerritories.IndexOf(stringVal);
        }

        private void _states_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {              
            var spinner = (Spinner)sender;
            var selectedStateText = Convert.ToString(spinner.GetItemAtPosition(e.Position));        

            try
            {
                if (_legislatorAdapter == null)
                {
                    Logger.Error($"Legislator adapater is null. Cannot select legislators for state ({selectedStateText}).");
                    return;
                }

                var stateAndTerrWithDesc = _statesAndTerrWithDescription.Where(s => s.Item2.Equals(selectedStateText)).FirstOrDefault();

                if (stateAndTerrWithDesc == null)
                {
                    Logger.Error($"Could not retrive the State for the selected {selectedStateText}.");
                    return;
                }

                _fragment.Activity.RunOnUiThread(() =>
                {
                  _legislatorAdapter.UpdateLegislators(_legislators.FilterByState(stateAndTerrWithDesc.Item1));
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Cannot select legislators for selected State ({selectedStateText}). Error: {ex.Message}");
            }
        }
    }
}