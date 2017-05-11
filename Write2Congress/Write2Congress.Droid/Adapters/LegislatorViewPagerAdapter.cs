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

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorViewPagerAdapter : FragmentPagerAdapter
    {
        public List<BaseRecyclerViewerFragment> viewers = new List<BaseRecyclerViewerFragment>();
        private Legislator _legislator;

        public LegislatorViewPagerAdapter(Android.Support.V4.App.FragmentManager fm, Legislator legislator)
            : base(fm)
        {
            //TODO RM: Find out how to implemt and where to create viewers. In parent fragment, or in adapter.
            //Also find how to retain instance on rotation. Also ensure there are no memory leak if using
            //FragmentPagerAdapter (since it keeps the fragment instance) and all references to activites
            //context and ect are cleared 
            _legislator = legislator;
            viewers.Add(CommitteeViewerFragmentCtrl.CreateInstance(legislator));
            viewers.Add(BillViewer.CreateInstance(legislator));
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