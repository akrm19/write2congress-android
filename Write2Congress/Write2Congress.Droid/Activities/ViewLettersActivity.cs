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
        public string ViewLettersActivityType;
        private BaseFragment _currentFragment;

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.viewLettersActv_parent;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_ViewLetters);
            SetupToolbar(Resource.Id.viewLettersActv_toolbar);
            SetupNavigationMenu(Resource.Id.viewLettersActv_navigationDrawer);

            _currentFragment = SupportFragmentManager.FindFragmentByTag(TagsType.ViewLettersFragment) as BaseFragment;

            if (_currentFragment == null)
            {
                var _fragmentType = GetFragmentTypeFromIntent();

                _currentFragment = GetNewFragmentByViewLettersFragType(_fragmentType);
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _currentFragment, Resource.Id.viewLettersActv_fragmentContainer, TagsType.ViewLettersFragment);
            }
        }

        private BaseFragment GetNewFragmentByViewLettersFragType(string fragmentType)
        {
            switch (fragmentType)
            {
                case ViewLettersFragmentType.Drafts:
                case ViewLettersFragmentType.Sent:
                    var baseViewLetterFrag = new BaseViewLetterFragment();
                    baseViewLetterFrag.Arguments = baseViewLetterFrag.Arguments ?? new Bundle();
                    baseViewLetterFrag.Arguments.PutString(BundleType.ViewLettersFragType, fragmentType);
                    return baseViewLetterFrag;
                default:
                    MyLogger.Error("ViewLettersFragmentType is not valid. Unable to create fragment");
                    return null;
            }
        }

        private string GetFragmentTypeFromIntent()
        {
            var fragType = ViewLettersFragmentType.Sent;
            if (Intent != null && Intent.HasExtra(BundleType.ViewLettersFragType))
                fragType = Intent.GetStringExtra(BundleType.ViewLettersFragType);

            return fragType;
        }

        protected override void OpenDrafts()
        {
            if (_currentFragment.GetType() == typeof(BaseViewLetterFragment)
                && !string.IsNullOrWhiteSpace(ViewLettersActivityType) 
                && ViewLettersActivityType.Equals(ViewLettersFragmentType.Drafts))
                return;

            ReplaceFragment(ViewLettersFragmentType.Drafts);
        }

        protected override void OpenSent()
        {
            if (_currentFragment.GetType() == typeof(BaseViewLetterFragment)
                && !string.IsNullOrWhiteSpace(ViewLettersActivityType) 
                && ViewLettersActivityType.Equals(ViewLettersFragmentType.Sent))
                return;

            ReplaceFragment(ViewLettersFragmentType.Sent);
        }

        private void ReplaceFragment(string newViewLettersFragmentType)
        {
            _currentFragment = GetNewFragmentByViewLettersFragType(newViewLettersFragmentType);
            var containerId = Resource.Id.viewLettersActv_fragmentContainer;

            ReplaceFragmentByTag(this, _currentFragment, containerId, TagsType.ViewLettersFragment);
        }
    }
}