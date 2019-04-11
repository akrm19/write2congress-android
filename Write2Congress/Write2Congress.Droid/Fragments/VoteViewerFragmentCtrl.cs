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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);
            _legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

			_voteManager = new VoteManager(MyLogger);
            currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);

            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            var adapter = new VoteAdapter(this);
            adapter.OnEndOfListReached += Adapter_OnEndOfListReached;
            recycler.SetAdapter(adapter);

			SetLoadingTextInEmptyView();

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

        void Adapter_OnEndOfListReached(object sender, EventArgs e)
        {
            if(_isThereMoreVotes)
                SetLoadMoreButtonVisibility(true);
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

                return new Tuple<List<Vote>, bool, int>(results.Results, results.IsThereMoreResults, localCurrentPage);
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

                        ShowVotes(_votes, _isThereMoreVotes);
                    }
                });
            });

            getVotesTask.Start();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (_votes != null)
            {
                var serializedVotes = _votes.SerializeToJson();
                outState.PutString(BundleType.Votes, serializedVotes);
            }

            outState.PutBoolean(BundleType.VotesIsThereMoreContent, _isThereMoreVotes);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (errorOccurred)
                HandleErrorRetrievingData();
            else if (_votes == null)
                SetLoadingTextInEmptyView();
            else
                ShowVotes(_votes, _isThereMoreVotes);
        }

        protected override void CleanUpReferencesToViewOrContext()
        {
            base.CleanUpReferencesToViewOrContext();
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

            if (IsBeingShown || recycler.GetAdapter() != null)
            {
                SetLoadMoreButtonInDisabledState(false);

                (recycler.GetAdapter() as VoteAdapter).UpdateVotes(_votes);
                SetLoadingUiOff();
            }
        }
    }
}