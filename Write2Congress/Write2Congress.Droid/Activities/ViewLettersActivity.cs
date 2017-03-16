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
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Android.Support.Design.Widget;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class ViewLettersActivity : BaseToolbarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_ViewLetters);

            using (var navigationView = FindViewById<NavigationView>(Resource.Id.viewLettersActv_navigationDrawer))
                navigationView.NavigationItemSelected += NavigationItemSelected;

            var viewLettersFragment = SupportFragmentManager.FindFragmentByTag(TagsType.DraftsFragment) as DraftLettersFragment;

            if (viewLettersFragment == null)
            {
                viewLettersFragment = new DraftLettersFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, viewLettersFragment, Resource.Id.viewLettersActv_fragmentContainer, TagsType.DraftsFragment);
            }
        }

        protected override void SetupOnCreateOptionsMenu(IMenu menu)
        {
            //throw new NotImplementedException();
        }
    }
}