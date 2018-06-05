
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var baseResults = base.OnCreateOptionsMenu(menu);

            using (var searchBills = menu.FindItem(SearchItemId))
            {
                searchBills.SetEnabled(false);
                searchBills.SetVisible(false);
            }

            return baseResults;
        }
    }
}
