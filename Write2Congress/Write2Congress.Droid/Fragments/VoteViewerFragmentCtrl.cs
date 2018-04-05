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
        private bool _isThereMoreVotes = true;
        private List<Vote> _votes;
        private LegislatorViewPagerAdapter _adapter;
        private Legislator _legislator;
        private VoteManager _voteManager;

        public VoteViewerFragmentCtrl() {}

        public static VoteViewerFragmentCtrl CreateInstance(Legislator legislator)
        {
            var newFragment = new VoteViewerFragmentCtrl();

            var args = new Bundle();
            args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            newFragment.Arguments = args;

            return newFragment;
        }

        /*
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
        */

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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _voteManager = new VoteManager(MyLogger);

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);
            _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

            currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = base.OnCreateView(inflater, container, savedInstanceState);
            
            recyclerAdapter = new VoteAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

			SetLoadingUi();

            if (_votes != null && _votes.Count() >= 0)
                SetVotes(_votes, _isThereMoreVotes);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Votes, string.Empty)))
            {
                var serializedVotes = savedInstanceState.GetString(BundleType.Votes);
                _votes = new List<Vote>().DeserializeFromJson(serializedVotes);
                SetVotes(_votes, _isThereMoreVotes);
            }
            else
                FetchMoreLegislatorContent(false);

            return fragment;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (errorOccurred)
                HandleErrorRetrievingData();
            if (_votes == null)
                SetLoadingUi();
            else
                ShowVotes(_votes, _isThereMoreVotes);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        
            if(_votes != null)
            {
                var serializedVotes = _votes.SerializeToJson();
                outState.PutString(BundleType.Votes, serializedVotes);
            }

			outState.PutBoolean(BundleType.VotesIsThereMoreContent, _isThereMoreVotes);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _votes = null;
            _voteManager = null;
            _legislator = null;
            //LoadMoreClick = null;
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
            SetVotes(votes, isThereMoreVotes);

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
            base.FetchMoreLegislatorContent(isNextClick);

            var getVotesTask = new Task<Tuple<List<Vote>, bool, int>>((prms) =>
            {
                var passedParams = prms as Tuple<string, VoteManager, int>;

                var legislatorId = passedParams.Item1;
                var vm = passedParams.Item2;
                var localCurrentPage = passedParams.Item3;

                var results = vm.GetLegislatorVotes(legislatorId, localCurrentPage);

                //TODO RM: Verify that ProPublica API does not indicate if there are more results
                //setting to true for now, since it seems ProPublica does not indicate that there are more results
                _isThereMoreVotes = true;// vm.IsThereMoreResultsForLastCall();

                return new Tuple<List<Vote>, bool, int>(results, _isThereMoreVotes, localCurrentPage);
            }, new Tuple<string, VoteManager, int>(_legislator.IdBioguide, _voteManager, currentPage));

            getVotesTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    if (antecedent.IsFaulted || antecedent.IsCanceled)
                        HandleErrorRetrievingData();
                    else
                    {
                        HandleSuccessfullDataRetrieval();

                        currentPage = antecedent.Result.Item3 + 1;
                        _isThereMoreVotes = antecedent.Result.Item2;

                        if (_votes == null || !_votes.Any())
                            _votes = antecedent.Result.Item1;
                        else
                            _votes.AddRange(antecedent.Result.Item1);

                        //TODO RM: Consolidate these two methods or rename them
                        SetLoadMoreButtonTextAsLoading(false);
                        ShowRecyclerButtons(_isThereMoreVotes);

                        ShowVotes(_votes, _isThereMoreVotes);
                    }
                });
            });

            getVotesTask.Start();            
        }
    }
}