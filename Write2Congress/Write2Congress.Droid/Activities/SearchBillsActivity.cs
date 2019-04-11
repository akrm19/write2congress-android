
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
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class SearchBillsActivity : ViewBillsBaseActivity
    {
        protected override BillViewerKind GetBillViewerKind
        {
            get
            {
                return BillViewerKind.BillSearch;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!string.IsNullOrWhiteSpace(CurrentSearch))
                UpdateTitleBarText($"'{CurrentSearch}' Bills");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var result = base.OnCreateOptionsMenu(menu);

            // If we have a SearchTerm (after roration) hide
            // search icon and show cancel search icon
            if (!string.IsNullOrWhiteSpace(CurrentSearch))
            {
                menu.FindItem(SearchItemId)?.SetVisible(false);
                menu.FindItem(ExitSearchItemId)?.SetVisible(true);
            }

            return result;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var result = base.OnOptionsItemSelected(item);

            switch (item.ItemId)
            {
                case Resource.Id.searchBillsMenu_exitSearch:
                    item.SetVisible(false);
                    SetToolbarSearchviewVisibility(true);
                    SetToolbarFilterviewVisibility(true);
                    _exitSearchClicked?.Invoke();
                    break;
                case Resource.Id.searchBillsMenu_search:
                    SetToolbarFilterviewVisibility(false);
                    SetToolbarExitSearchviewVisibility(false);
                    break;
            }

            return result;
        }

        //Id to locate the Menu axml file used for top bar
        protected override int MenuItemId => Resource.Menu.menu_searchBills;

        protected override int DrawerMenuItemId => Resource.Id.actionMenu_searchBills;

        protected override int SearchItemId => Resource.Id.searchBillsMenu_search;

        protected override int FilterDataItemId => 0;//Resource.Id.searchBillsMenu_filter;

        protected override int ExitSearchItemId => Resource.Id.searchBillsMenu_exitSearch;
    }
}
