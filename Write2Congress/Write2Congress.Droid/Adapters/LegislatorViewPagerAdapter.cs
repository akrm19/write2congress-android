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
using Write2Congress.Shared.BusinessLayer;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;

namespace Write2Congress.Droid.Adapters
{
    public class LegislatorViewPagerAdapter : FragmentStatePagerAdapter
    {
        private Legislator _legislator;

        private List<Vote> _votes;
        private List<Bill> _billsSponsored;
        private List<Bill> _billsCosponsored;
        private List<Committee> _committees;
        //public List<BaseRecyclerViewerFragment> viewers = new List<BaseRecyclerViewerFragment>();

        BaseRecyclerViewerFragment _voteFrag;
        BaseRecyclerViewerFragment _sponsoredBillFrag;
        BaseRecyclerViewerFragment _cosponsoredBillFrag;
        BaseRecyclerViewerFragment _committeesFrag;

        List<BaseRecyclerViewerFragment> _frags = new List<BaseRecyclerViewerFragment>(System.Enum.GetNames(typeof(ViewPagerList)).Length);

        public LegislatorViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        public List<Vote> Votes
        {
            get { return _votes; }
            set { _votes = value; }
        }

        public List<Bill> BillsSponsored
        {
            get { return _billsSponsored; }
            set { _billsSponsored = value; }
        }

        public List<Bill> BillsCosponsored
        {
            get { return _billsCosponsored; }
            set { _billsCosponsored = value; }
        }

        public List<Committee> Committees
        {
            get { return _committees; }
            set { _committees = value; }
        }

        public LegislatorViewPagerAdapter(Android.Support.V4.App.FragmentManager fm, Legislator legislator)
            : base(fm)
        {
            _legislator = legislator;

            /*
             * Find how to retain instance on rotation. Also ensure there are no memory leak if using
            //FragmentPagerAdapter (since it keeps the fragment instance) and all references to activites
            //context and ect are cleared 
            //viewers.Insert((int)ViewPagerList.LegislatorVotes, VoteViewerFragmentCtrl.CreateInstance(legislator, LoadMoreVotesClicked));

            ////// Look into
            ////viewers.Insert(0, VoteViewerFragmentCtrl.CreateInstance(legislator, LoadMoreVotesClicked));


            /////// Move back to last place
            //viewers[(int)ViewPagerList.LegislatorCommittees] = CommitteeViewerFragmentCtrl.CreateInstance(legislator);
            //viewers[(int)ViewPagerList.LegislatorCommittees] = CommitteeViewerFragmentCtrl.CreateInstance(legislator);
            //viewers.Add(CommitteeViewerFragmentCtrl.CreateInstance(legislator));

            /////// For some reason this cause issues, it could be the use of an async further 
            //down in the BillViewerFragmentCtrl code that calls the web svc
            ////viewers.Insert((int)ViewPagerList.LegislatorBillsSponsored, BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.SponsoredBills));
            //viewers.Insert(1, BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.SponsoredBills));


            //viewers.Add(BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.SponsoredBills));
            //viewers.Add(BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.CosponsoredBills));
            ///viewers.Insert((int)ViewPagerList.LegislatorBillsCosponsored, BillViewerFragmentCtrl.CreateInstance(legislator, BillViewerKind.CosponsoredBills));


            ////viewers.Insert((int)ViewPagerList.LegislatorCommittees, CommitteeViewerFragmentCtrl.CreateInstance(legislator));
            /////// RM Look into
            ////viewers.Insert(1, CommitteeViewerFragmentCtrl.CreateInstance(legislator));
            */
        }

        public override Fragment GetItem(int position)
        {
            return GetItem((ViewPagerList)position);
        }

        public Fragment GetExitsingItem(ViewPagerList viewpagerType)
        {
            switch (viewpagerType)
            {
                case ViewPagerList.LegislatorVotes:
                    return _voteFrag;
                case ViewPagerList.LegislatorBillsSponsored:
                    return _sponsoredBillFrag;
                case ViewPagerList.LegislatorBillsCosponsored:
                    return _cosponsoredBillFrag;
                case ViewPagerList.LegislatorCommittees:
                    return _committeesFrag;
                default:
                    return null;
            }
        }

        public Fragment GetItem(ViewPagerList viewpagerType)
        {
            switch (viewpagerType)
            {
                case ViewPagerList.LegislatorVotes:
                    return VoteViewerFragmentCtrl.CreateInstance(_legislator);
                case ViewPagerList.LegislatorBillsSponsored:
                    return BillViewerFragmentCtrl.CreateInstance(_legislator, BillViewerKind.SponsoredBills);
                case ViewPagerList.LegislatorBillsCosponsored:
                    return BillViewerFragmentCtrl.CreateInstance(_legislator, BillViewerKind.CosponsoredBills); 
                case ViewPagerList.LegislatorCommittees:
                    return CommitteeViewerFragmentCtrl.CreateInstance(_legislator);
                default:
                    return null;
            }
        }

        //public Android.Support.V4.App.Fragment GetFragment(ViewPagerList viewpagerType)
        //{
        //    return viewers[(int)viewpagerType];
        //}

        //public Android.Support.V4.App.Fragment GetFragment(int position)
        //{
        //    return viewers[position];
        //}

        public override int Count
        {
            get
            {
                return System.Enum.GetNames(typeof(ViewPagerList)).Length;
            }
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            //var frag = (Fragment)base.InstantiateItem(container, position);

            //TODO RM: THIS will THROW AN ERROR BECAUSE THE _frags list is empty
			//_frags could be used lated to store created fragmetns instead of instantiating them each time
            //_frags[position] = frag as BaseRecyclerViewerFragment;
            //var baseFrag = frag as BaseRecyclerViewerFragment;
            //_frags.Insert(position, baseFrag);
            return base.InstantiateItem(container, position);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            var viewPageType = (ViewPagerList)position;
            string title = string.Empty;

            switch (viewPageType)
            {
                case ViewPagerList.LegislatorVotes:
                    title = "Votes";
                    break;
                case ViewPagerList.LegislatorBillsSponsored:
                    title = "Bills Sponsored";
                    break;
                case ViewPagerList.LegislatorBillsCosponsored:
                    title = "Bills Cosponsored";
                    break;
                case ViewPagerList.LegislatorCommittees:
                    title = "Committtees";
                    break;
                default:
                    break;
            }
            
            return new Java.Lang.String(title);
            //return new Java.Lang.String(viewers[position].ViewerTitle());
        }
    }
}