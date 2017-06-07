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
        private VoteManager _voteManager;
        private List<Vote> _votes= new List<Vote>();
        private Legislator _legislator;
        private int _currentPage = 1;

        public VoteViewerFragmentCtrl() {}

        public static VoteViewerFragmentCtrl CreateInstance(Legislator legislator)
        {
            var newFragment = new VoteViewerFragmentCtrl();

            var args = new Bundle();
            args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            newFragment.Arguments = args;

            return newFragment;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (_votes != null)
            {
                var serializedVotes = _votes.SerializeToJson();
                outState.PutString(BundleType.Votes, serializedVotes);
            }

            outState.PutInt(BundleType.CurrentPage, _currentPage);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var serializedLegislator = Arguments.GetString(BundleType.Legislator);
            _legislator = new Legislator().DeserializeFromJson(serializedLegislator);

            _voteManager = new VoteManager(MyLogger);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new VoteAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();

            if(savedInstanceState!= null)
                _currentPage = savedInstanceState.GetInt(BundleType.CurrentPage, 1);

            if (_votes != null && _votes.Count > 0)
                ShowVotes(_votes);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Votes, string.Empty)))
            {
                var serializedVotes = savedInstanceState.GetString(BundleType.Votes);
                _votes = new List<Vote>().DeserializeFromJson(serializedVotes);
                ShowVotes(_votes);
            }
            else
            {
                FetchLegislatorVotes(false);
            }

            return fragment;
        }

        private void FetchLegislatorVotes(bool isNextClick)
        {
            _currentPage = isNextClick 
                ? _currentPage + 1 
                : _currentPage;

            var getVotesTask = new Task<Tuple<List<Vote>, bool>>((prms) =>
            //var getVotesTask = new Task<List<Vote>>((prms) =>
            {
                var passedParams = prms as Tuple<string, VoteManager, int>;

                var legislatorId = passedParams.Item1;
                var vm = passedParams.Item2;
                var currentPage = passedParams.Item3;

                var results = vm.GetLegislatorVotes(legislatorId, currentPage);
                var isThereMoreVotes = vm.IsThereMoreResultsForLastCall();
                //return results;
                return new Tuple<List<Vote>, bool>(results, isThereMoreVotes);
            }, new Tuple<string, VoteManager, int>(_legislator.BioguideId, _voteManager, _currentPage));

            getVotesTask.ContinueWith((antecedent) =>
            {
                if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                    return;

                Activity.RunOnUiThread(() =>
                {
                    
                    var isThereMoreVotes2 = antecedent.Result;
                    var isThereMoreVotes = antecedent.Result.Item2;

                    if (isThereMoreVotes)
                        _votes.AddRange(antecedent.Result.Item1);
                    else
                        _votes = antecedent.Result.Item1;
                    ShowRecyclerButtons(isThereMoreVotes);
                        
                    //_votes = antecedent.Result;
                    ShowVotes(_votes);
                });
            });
            getVotesTask.Start();
        }

        public void ShowVotes(List<Vote> votes)
        {
            (recyclerAdapter as VoteAdapter).UpdateVotes(votes);

            SetLoadingUiOff();
        }

        protected override void NextButon_Click(object sender, EventArgs e)
        {
            base.NextButon_Click(sender, e);

            FetchLegislatorVotes(true);
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyVotesText);
        }

        public override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.votes);
        }
    }
}