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

namespace Write2Congress.Droid.Fragments
{
    public class MainFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            var mainFragment = inflater.Inflate(Resource.Layout.frag_Main, container, false);



            var legislatorManager = new LegislatorManager();

            // Set our view from the "main" layout resource


            // Get our button from the layout resource,
            // and attach an event to it
            Button button = mainFragment.FindViewById<Button>(Resource.Id.mainFrag_myButton);
            var zipInput = mainFragment.FindViewById<EditText>(Resource.Id.mainFrag_zip);
            var resultText = mainFragment.FindViewById<TextView>(Resource.Id.mainFrag_result);


            button.Click += delegate {
                var zip = zipInput.Text;

                var legislators = legislatorManager.GetLegislatorByZipcode(zip);

                foreach (Legislator legislator in legislators)
                {

                    var legislatorText = JsonConvert.SerializeObject(legislator);
                    resultText.Text += legislatorText;
                }

            };


            return mainFragment;
        }
    }
}