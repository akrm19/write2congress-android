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
        private string _fragmentType;
        private BaseFragment _currentFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_ViewLetters);
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
                    return new DraftLettersFragment();
                case ViewLettersFragmentType.Sent:
                    return new SentLettersFragment();
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
            if (_currentFragment.GetType() == typeof(DraftLettersFragment))
                return;

            ReplaceFragment(ViewLettersFragmentType.Drafts);
        }

        protected override void OpenSent()
        {
            if (_currentFragment.GetType() == typeof(SentLettersFragment))
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