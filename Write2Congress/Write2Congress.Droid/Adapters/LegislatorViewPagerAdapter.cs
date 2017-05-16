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
using Android.Support.V4.App;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorViewPagerAdapter : FragmentPagerAdapter
    {
        public List<BaseRecyclerViewerFragment> viewers = new List<BaseRecyclerViewerFragment>();

        public LegislatorViewPagerAdapter(Android.Support.V4.App.FragmentManager fm, Legislator legislator)
            : base(fm)
        {
            //TODO RM: Find how to retain instance on rotation. Also ensure there are no memory leak if using
            //FragmentPagerAdapter (since it keeps the fragment instance) and all references to activites
            //context and ect are cleared 

            viewers.Add(CommitteeViewerFragmentCtrl.CreateInstance(legislator));

            //TODO RM: For some reason this cause issues, it could be the use of an async further 
            //down in the BillViewerFragmentCtrl code that calls the web svc
            viewers.Add(BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.CosponsoredBills));
            viewers.Add(BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.SponsoredBills));
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return viewers[position];
        }

        public Android.Support.V4.App.Fragment GetFragment(int position)
        {
            return viewers[position];
        }

        public override int Count
        {
            get
            {
                return viewers.Count;
            }
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(viewers[position].ViewerTitle());
        }
    }
}