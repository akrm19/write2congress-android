using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Write2Congress.Shared.BusinessLayer;
using Newtonsoft.Json;
using Write2Congress.Shared.DomainModel;

namespace Write2Congress.Droid
{
	[Activity (Label = "Write2Congress.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            var legislatorManager = new LegislatorManager();
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
            var zipInput = FindViewById<EditText>(Resource.Id.zip);
            var resultText = FindViewById<TextView>(Resource.Id.result);


			button.Click += delegate {
                var zip = zipInput.Text;

                var legislators = legislatorManager.GetLegislatorByZipcode(zip);

                foreach(Legislator legislator in legislators)
                {

                    var legislatorText = JsonConvert.SerializeObject(legislator);
                    resultText.Text += legislatorText;
                }

            };
		}

        private void Test()
        {

        }
	}
}


