
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
    }
}
