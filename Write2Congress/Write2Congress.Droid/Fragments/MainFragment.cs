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

namespace Write2Congress.Droid.Fragments
{
    public class MainFragment : BaseFragment
    {
        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        LegislatorAdapter legislatorAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var legislatorManager = new LegislatorManager();
            layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);


            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);

            recyclerView = mainFragment.FindViewById<RecyclerView>(Resource.Id.mainFrag_legislatorsParentRecycler);
            recyclerView.SetLayoutManager(layoutManager);

            var button = mainFragment.FindViewById<Button>(Resource.Id.mainFrag_myButton);
            var zipInput = mainFragment.FindViewById<EditText>(Resource.Id.mainFrag_zip);


            button.Click += delegate {
                var zip = zipInput.Text;

                var legislators = legislatorManager.GetLegislatorByZipcode(zip);
                var legislatorAdapter = new LegislatorAdapter(this, legislators);

                recyclerView.SetAdapter(legislatorAdapter);
            };


            return mainFragment;
        }
    }
}