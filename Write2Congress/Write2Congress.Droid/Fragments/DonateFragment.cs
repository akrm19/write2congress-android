
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

namespace Write2Congress.Droid.Fragments
{
    public class DonateFragment : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //This is needed to tell host activity that the fragment as menu options to add
            HasOptionsMenu = true;

            var frag =  inflater.Inflate(Resource.Layout.frag_Donate, container, false);

            using(var donateToDevButton = frag.FindViewById<TextView>(Resource.Id.donateFrag_donateToDev))
                donateToDevButton.Click += DonateToDevButton_Click;

            using (var donateToProPublicaButton = frag.FindViewById<TextView>(Resource.Id.donateFrag_donateToProPublic))
                donateToProPublicaButton.Click += DonateToProPublica_Click;

            using (var contributeToUsIoButton = frag.FindViewById<TextView>(Resource.Id.donateFrag_contributeToUsIoProject))
                contributeToUsIoButton.Click += ContributeToUsIoProj_Click;

            return frag;
        }

        void DonateToDevButton_Click(object sender, EventArgs e)
        {
            var url = @"http://www.techhops.com";
            LaunchViewWebsiteIntent(url);
        }

        void DonateToProPublica_Click(object sender, EventArgs e)
        {
            var url = @"https://donate.propublica.org";
            LaunchViewWebsiteIntent(url);
        }

        void ContributeToUsIoProj_Click(object sender, EventArgs e)
        {
            var url = @"https://theunitedstates.io/";
            LaunchViewWebsiteIntent(url);
        }

        private void LaunchViewWebsiteIntent(string url)
        {
            var intent = new Intent(Intent.ActionView);
            var uri = Android.Net.Uri.Parse(url);

            intent.SetData(uri);

            StartActivity(intent);
        }
    }
}
