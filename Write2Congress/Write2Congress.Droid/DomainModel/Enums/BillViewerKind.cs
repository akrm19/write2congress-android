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

namespace Write2Congress.Droid.DomainModel.Enums
{
    public enum BillViewerKind
    {
        SponsoredBills,
        CosponsoredBills, 
        AllBillsOfEveryone
    }
}