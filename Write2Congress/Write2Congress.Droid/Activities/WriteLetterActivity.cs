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
    [Activity(Label = "WriteLetterActivity")]
    public class WriteLetterActivity : BaseActivity
    {
        private WriteLetterFragment _writeLetterFragment;
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_WriteLetter);
            //SetupToolbar(Resource.Id.writeLetterActv_toolbar);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.writeLetterActv_parent);
            _navigationView = FindViewById<NavigationView>(Resource.Id.writeLetterActv_navigationDrawer);
            _navigationView.NavigationItemSelected += NavigationItemSelected;

            _writeLetterFragment = FragmentManager.FindFragmentByTag<WriteLetterFragment>(TagsType.WriteLetterFragment);

            if(_writeLetterFragment == null)
            {
                _writeLetterFragment = new WriteLetterFragment();
                AndroidHelper.AddFragment(FragmentManager, _writeLetterFragment, Resource.Id.writeLetterActv_fragmentContainer, TagsType.WriteLetterFragment);
            }
        }

        private void NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //throw new NotImplementedException();
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
                Logger.Error($"Unable to deserialize legislator from string: {serializedLegislator}");
                return null;
            }
        }
    }
}