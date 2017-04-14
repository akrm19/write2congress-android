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
using Newtonsoft.Json;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Code;
using Android.Support.V4.View;
using SearchView = Android.Support.V7.Widget.SearchView;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Droid.Interfaces;
using Android.Support.Design.Widget;
using Write2Congress.Droid.DomainModel.Enums;
using Android.Support.V7.App;

namespace Write2Congress.Droid.Activities
{
    [Activity(MainLauncher =true)]
    public class MainActivity : BaseToolbarActivity, ILegislatorViewerActivity
    {
        private MainFragment _mainFragment;
        private SearchTextChangedDelegate _legislatorSearchTextChanged;

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.mainActv_parent;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.actv_Main);

            SetupToolbar(Resource.Id.mainActv_toolbar);
            SetupNavigationMenu(Resource.Id.mainActv_navigationDrawer);

            _mainFragment = SupportFragmentManager.FindFragmentByTag(TagsType.MainParentFragment) as MainFragment;

            if(_mainFragment == null)
            {
                _mainFragment = new MainFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _mainFragment, Resource.Id.mainActv_fragmentContainer, TagsType.MainParentFragment);
            }
        }

        private void ActionMenu_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.actionMenu_search:
                    Toast.MakeText(ApplicationContext, "search", ToastLength.Short).Show();
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_writeNew:
                    AppHelper.StartWriteNewLetterIntent(this, BundleSenderKind.LegislatorViewer);
                    return true;
                case Resource.Id.mainMenu_refresh:
                    UpdateLegislatorsWithPrompt();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void UpdateLegislatorsWithPrompt()
        {
            if (!LegislatorsUpdatedInLast30Days())
                UpdateLegislators();
            else
                VerifyUserWantsToUpdateLegislators();
        }

        private void UpdateLegislators()
        {
            var results = GetBaseApp().UpdateLegislatorData();
            var message = AndroidHelper.GetString(results
                ? Resource.String.legislatorDataSuccessfullyUpdated
                : Resource.String.unableToUpdateLegislatorData);

            ShowToast(message);
        }

        private void ShowToast(string message, ToastLength lenght = ToastLength.Short)
        {
            Toast.MakeText(ApplicationContext, message, lenght).Show();
        }

        private bool LegislatorsUpdatedInLast30Days()
        {
            var lastUpdate = AppHelper.GetLastLegislatorUpdate();

            return lastUpdate == DateTime.MinValue
                ? true
                : lastUpdate.CompareTo(DateTime.Now.AddDays(-30)) >= 0;
        }

        private void VerifyUserWantsToUpdateLegislators()
        {
            var lastUpdate = AppHelper.GetLastLegislatorUpdate();

            var message = string.Format("{0}{1}{1}{2}{1}{3}",
                AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorData),
                System.Environment.NewLine,
                AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorDataWarning),
                lastUpdate == DateTime.MinValue
                    ? string.Empty
                    : $"{AndroidHelper.GetString(Resource.String.verifyUpdateOfLegislatorDataLastUpdate)}: {lastUpdate.ToString("G")}");

            //TODO RM: Further prettify dialog
            var verifyPrompt = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.VerifyDialogTheme);
            verifyPrompt.SetTitle(AndroidHelper.GetString(Resource.String.confirmRefresh));
            verifyPrompt.SetMessage(message);
            verifyPrompt.SetNegativeButton(Resource.String.dismiss,
                (sender, args) => 
                    {
                        RunOnUiThread(() => (sender as Android.Support.V7.App.AlertDialog).Dismiss());
                    });
            verifyPrompt.SetPositiveButton(Resource.String.ok,
                (sender, args) =>
                {
                    RunOnUiThread(UpdateLegislators);
                });

            verifyPrompt.Create().Show();
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            using (var searchMenuitem = menu.FindItem(Resource.Id.mainMenu_search))
            using (var searchView = MenuItemCompat.GetActionView(searchMenuitem))
            using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                searchViewJavaObj.QueryTextChange += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.NewText);
                };

                searchViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.Query);
                    e.Handled = true;
                };
            }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            _legislatorSearchTextChanged = null;

            base.OnDestroy();
        }

        public SearchTextChangedDelegate LegislatorSearchTextChanged
        {
            get
            {
                return _legislatorSearchTextChanged;
            }
            set
            {
                _legislatorSearchTextChanged += value;
            }
        }

        public void ClearLegislatorSearchTextChangedDelegate()
        {
            _legislatorSearchTextChanged = null;
        }
    }
}