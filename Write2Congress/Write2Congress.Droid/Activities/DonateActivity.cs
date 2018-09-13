using System;
using Android.App;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class DonateActivity : BaseToolbarActivity
    {
        private DonateFragment _donateFragment;
        
        public DonateActivity()
        {
        }

        protected override int DrawerLayoutId => Resource.Id.donateActv_parent;

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_Donate);
            SetupToolbar(Resource.Id.donateActv_toolbar);//, AndroidHelper.GetString(Resource.String.donate));
            SetupNavigationMenu(Resource.Id.donateActv_navigationDrawer);

            _donateFragment = SupportFragmentManager.FindFragmentByTag(TagsType.DonateFragment) as DonateFragment;

            if(_donateFragment == null)
            {
                _donateFragment = new DonateFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _donateFragment, Resource.Id.donateActv_fragmentContainer, TagsType.DonateFragment);
            }
        }
    }
}
