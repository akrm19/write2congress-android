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
        //LegislatorAdapter _legislatorAdapter;

        Spinner _states;
        Address _currentAddress;

        List<string> _stateNames;
        List<Legislator> _legislators;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var legislatorManager = new LegislatorManager();
            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);

            // Use this to return your custom view for this Fragment
            _layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            _states = mainFragment.FindViewById<Spinner>(Resource.Id.mainFrag_states);
            _recyclerView = mainFragment.FindViewById<RecyclerView>(Resource.Id.mainFrag_legislatorsParentRecycler);
            _recyclerView.SetLayoutManager(_layoutManager);

            _stateNames = Enum.GetNames(typeof(Shared.DomainModel.Enum.StateOrTerritory)).ToList();
            _currentAddress = GeoHelper.GetCurrentAddress();

            var button = mainFragment.FindViewById<Button>(Resource.Id.mainFrag_myButton);
            var searchInputTest = mainFragment.FindViewById<EditText>(Resource.Id.mainFrag_zip);

            //var statesAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, _stateNames);
            //_states.Adapter = statesAdapter;
            //_states.ItemSelected += _states_ItemSelected;

            _legislators = AppHelper.GetCachedLegislators();
            var legislatorAdapter = new LegislatorAdapter(this, _legislators);
            _recyclerView.SetAdapter(legislatorAdapter);
            //using (var legislatorAdapter = new LegislatorAdapter(this, _legislators))
            //    _recyclerView.SetAdapter(legislatorAdapter);

            if (_currentAddress != null)
            {
                searchInputTest.Text = _currentAddress.PostalCode;
            }

            button.Click += delegate {
                _legislators = AppHelper.GetCachedLegislators().FilterByFirstMiddleOrLastName(searchInputTest.Text);
                var legislatorAdapter2 = new LegislatorAdapter(this, _legislators);
                _recyclerView.SetAdapter(legislatorAdapter2);
            };

            return mainFragment;
        }

        private void _states_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            var selectedState = Convert.ToString(spinner.GetItemAtPosition(e.Position));

            StateOrTerritory stateOrTerritory;

            if(Enum.TryParse<StateOrTerritory>(selectedState, out stateOrTerritory))
            {
                var _legislators = AppHelper.GetCachedLegislators().FilterByState(stateOrTerritory);
                //using (var legislatorAdapter = _recyclerView.GetAdapter() as LegislatorAdapter)
                //{
                //
                //    //legislatorAdapter.UpdateLegislators(legislators);
                //}
            }
            Toast.MakeText(this.Context, selectedState, ToastLength.Short).Show();
        }
    }
}