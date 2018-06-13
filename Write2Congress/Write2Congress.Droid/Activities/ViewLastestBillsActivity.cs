
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
    [Activity(Label = "ViewLastestBillsActivity")]
    public class ViewLastestBillsActivity : ViewBillsBaseActivity
    {
        protected override BillViewerKind GetBillViewerKind
        {
            get
            {
                return BillViewerKind.LastestBillsForEveryone;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        /*
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            using (var searchBills = menu.FindItem(SearchItemId))
            {
                searchBills.SetEnabled(false);
                searchBills.SetVisible(false);
            }

            using(var exitSearch = menu.FindItem(ExitSearchItemId))
            {
                exitSearch.SetEnabled(false);
                exitSearch.SetVisible(false);
            }

            //SetupFilterMenuItem(menu);
            return true;
        }
        */

        protected override int MenuItemId => Resource.Menu.menu_viewBills;

        protected override int FilterDataItemId => Resource.Id.viewLatestBillsMenu_filter;
    }
}
