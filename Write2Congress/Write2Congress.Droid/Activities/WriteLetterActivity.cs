using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class WriteLetterActivity : BaseToolbarActivity
    {
        private WriteLetterFragment _writeLetterFragment;
        private DrawerLayout _drawerLayout;
        //private NavigationView _navigationView; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_WriteLetter);

            using (var navigationView = FindViewById<NavigationView>(Resource.Id.writeLetterActv_navigationDrawer))
                navigationView.NavigationItemSelected += NavigationItemSelected;

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.writeLetterActv_parent);
            _writeLetterFragment = SupportFragmentManager.FindFragmentByTag(TagsType.WriteLetterFragment) as WriteLetterFragment;

            if(_writeLetterFragment == null)
            {
                var letter = GetLetterFromIntent();
                _writeLetterFragment = new WriteLetterFragment(letter);

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _writeLetterFragment, Resource.Id.writeLetterActv_fragmentContainer, TagsType.WriteLetterFragment);
            }
        }

        private Letter GetLetterFromIntent()
        {
            if (Intent == null || !Intent.HasExtra(BundleType.Letter))
                return null;

            var serialziedLetter = Intent.GetStringExtra(BundleType.Letter);

            var letter = new Letter().DeserializeFromJson(serialziedLetter);
            return letter;
        }

        private Letter GetLetterFromIntentBundle(Bundle bundle)
        {
            if (bundle == null || !bundle.ContainsKey(BundleType.Letter))
                return null;

            var serialziedLetter = bundle.GetString(BundleType.Letter);

            var letter = new Letter().DeserializeFromJson(serialziedLetter);
            return letter;
        }

        private Legislator GetLegislatorFromIntent()
        {
            var serializedLegislator = Intent.Extras.ContainsKey(BundleType.Legislator)
                ? Intent.GetStringExtra(BundleType.Legislator)
                : string.Empty;

            try
            {
                return string.IsNullOrWhiteSpace(serializedLegislator)
                    ? null
                    : new Legislator().DeserializeFromJson(serializedLegislator);
            }
            catch (Exception ex)
            {
                MyLogger.Error($"Unable to deserialize legislator from string: {serializedLegislator}");
                return null;
            }
        }
    }
}