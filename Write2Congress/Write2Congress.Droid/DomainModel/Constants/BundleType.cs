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

namespace Write2Congress.Droid.DomainModel.Constants
{
    public static class BundleType
    {
        public const string Legislator = "Legislator";
        public const string Letter = "Letter";
        public const string ViewLettersFragType = "ViewLettersFragType";
        public const string Sender = "Sender";
        public const string Committees = "Committees";
        public const string SponsoredBills = "SponsoredBills";
        public const string SponsoredBillsCurrentPage = "SponsoredBillsCurrentPage";
        public const string CosponsoredBills = "CosponsoredBills";
        public const string CosponsoredBillsCurrentPage = "CosponsoredBillsCurrentPage";
        public const string BillsIsThereMoreContent = "BillsIsThereMoreContent";
        public const string BillSearchResultsCurrentPage = "BillSearchResultsCurrentPage";
        public const string Bills = "Bills";
        public const string BillViewerFragmentType = "BillViewerFragmentType";
        public const string Votes = "Votes";
        public const string VotesCurrentPage = "VotesCurrentPage";
        public const string VotesIsThereMoreContent = "VotesIsThereMoreContent";
        public const string CurrentPage = "CurrentPage";
    }
}