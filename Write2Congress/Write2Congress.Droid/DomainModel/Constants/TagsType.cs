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
    public class TagsType
    {
        public const string MainParentFragment = "MainParentFragment";
        public const string WriteLetterFragment = "WriteLetterFragment";
        public const string ViewLettersFragment = "ViewLettersFragment";
        public const string DraftsFragment = "DraftsFragment";
        public const string SentFragment = "SentFragment";
        internal const string SettingsFragment = "SettingsFragment";
        internal const string SponsoredBillsFragment = "SponsoredBillsFragment";
        internal const string ViewLegislatorsFragment = "ViewLegislatorsFragment";
        internal const string VoteViewerFragment = "VoteViewerFragment";
    }
}