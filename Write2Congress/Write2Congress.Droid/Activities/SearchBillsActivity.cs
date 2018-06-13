
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
    [Activity(Label = "SearchBillsActivity")]
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

            // Create your application here
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
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
                case Resource.Id.searchBillsMenu_filter:
                    SetToolbarSearchviewVisibility(false);
                    SetToolbarExitSearchviewVisibility(false);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override int MenuItemId => Resource.Menu.menu_searchBills;

        protected override int SearchItemId => Resource.Id.searchBillsMenu_search;

        protected override int FilterDataItemId => Resource.Id.searchBillsMenu_filter;

        protected override int ExitSearchItemId => Resource.Id.searchBillsMenu_exitSearch;
    }
}
