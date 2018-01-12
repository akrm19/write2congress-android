using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Adapters;
using Write2Congress.Droid.Code;
using System.Threading.Tasks;

namespace Write2Congress.Droid.Fragments
{
    public class VoteViewerFragmentCtrl : BaseRecyclerViewerFragment
    {
        private bool _isThereMoreVotes;
        private List<Vote> _votes;
        private LegislatorViewPagerAdapter _adapter;

        public VoteViewerFragmentCtrl() {}

        public static VoteViewerFragmentCtrl CreateInstance(Legislator legislator)
        {
            var newFragment = new VoteViewerFragmentCtrl();

            var args = new Bundle();
            args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            newFragment.Arguments = args;

            return newFragment;
        }

        public static VoteViewerFragmentCtrl CreateInstance(Legislator legislator, List<Vote> votes, Action loadMoreClicked)
        {
            var newFragment = new VoteViewerFragmentCtrl();

            var args = new Bundle();

            if(legislator != null)
                args.PutString(BundleType.Legislator, legislator.SerializeToJson());

            if (votes != null)
                args.PutString(BundleType.Votes, votes.SerializeToJson());

            newFragment.Arguments = args;

            return newFragment;
        }

        public static VoteViewerFragmentCtrl CreateInstance(List<Vote> votes)
        {
            var newFragment = new VoteViewerFragmentCtrl();

            var args = new Bundle();
            args.PutString(BundleType.Votes, votes.SerializeToJson());
            newFragment.Arguments = args;

            return newFragment;
        }

        //public void SetOnClickListener(Action<bool> listener)
        //{
        //    if(!listenerSet)
        //        LoadMoreVoteClick = listener;
        //
        //    listenerSet = true;
        //}

        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = base.OnCreateView(inflater, container, savedInstanceState);
            
            recyclerAdapter = new VoteAdapter(this);
            recycler.SetAdapter(recyclerAdapter);
            
            //RetrieveCurrentPageIfAvailable(savedInstanceState);

            if (_votes == null)// && _votes.Count > 0)
                SetLoadingUi();
            else
                SetVotes(_votes, _isThereMoreVotes);

            //else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Votes, string.Empty)))
            //{
            //    var serializedVotes = savedInstanceState.GetString(BundleType.Votes);
            //    _votes = new List<Vote>().DeserializeFromJson(serializedVotes);
            //    ShowVotes(_votes);
            //}
            //else
            //    FetchMoreLegislatorContent(false);

            return fragment;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_votes == null)// && _votes.Count > 0)
                SetLoadingUi();
            else
                ShowVotes(_votes, _isThereMoreVotes);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);


            outState.PutBoolean(BundleType.VotesIsThereMoreContent, _isThereMoreVotes);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _votes = null;
            LoadMoreClick = null;
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyVotesText);
        }

        public override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.votes);
        }

        public void SetVotes(List<Vote> votes, bool isThereMoreVotes)
        {
            _votes = votes;
            _isThereMoreVotes = isThereMoreVotes;
        }

        public void ShowVotes(List<Vote> votes, bool isThereMoreVotes)
        {
            _votes = votes;
            _isThereMoreVotes = isThereMoreVotes;

            if (IsBeingShown || recyclerAdapter !=  null)
            {
                SetLoadMoreButtonTextAsLoading(false);
                ShowRecyclerButtons(_isThereMoreVotes);

                (recyclerAdapter as VoteAdapter).UpdateVotes(_votes);

                SetLoadingUiOff();
            }
        }

        protected override void FetchMoreLegislatorContent(bool isNextClick)
        {
            LoadMoreClick?.Invoke(true);
        }
    }
}