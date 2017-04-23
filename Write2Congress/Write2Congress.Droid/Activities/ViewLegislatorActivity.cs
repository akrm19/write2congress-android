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
using Write2Congress.Droid.DomainModel.Constants;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class ViewLegislatorActivity : BaseToolbarActivity
    {
        private ViewLegislatorFragment _viewLegislatorFragment;

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.viewLegislatorActv_parent;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actv_ViewLegislator);

            SetupToolbar(Resource.Id.viewLegislatorActv_toolbar);
            SetupNavigationMenu(Resource.Id.viewLegislatorActv_navigationDrawer);

            _viewLegislatorFragment = SupportFragmentManager.FindFragmentByTag(TagsType.ViewLegislatorsFragment) as ViewLegislatorFragment;

            if(_viewLegislatorFragment == null)
            {
                var serializedLegislator = AndroidHelper.GetStringFromIntent(Intent, BundleType.Legislator); 

                _viewLegislatorFragment = new ViewLegislatorFragment();

                //TODO RM:
                //http://stackoverflow.com/questions/9245408/best-practice-for-instantiating-a-new-android-fragment

                if(_viewLegislatorFragment.Arguments == null)
                    _viewLegislatorFragment.Arguments = new Bundle();

                _viewLegislatorFragment.Arguments.PutString(BundleType.Legislator, serializedLegislator);

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _viewLegislatorFragment, Resource.Id.viewLegislatorActv_fragmentContainer, TagsType.ViewLegislatorsFragment);
            }
        }
    }
}