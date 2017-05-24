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
        private List<Vote> _votes;
        private Legislator _legislator;

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
                var getVotesTask = new Task<List<Vote>>((prms) =>
                {
                    var legislatorId = (prms as Tuple<string, VoteManager>).Item1;
                    var vm = (prms as Tuple<string, VoteManager>).Item2;
                    return vm.GetLegislatorVotes(legislatorId, 1);
                }, new Tuple<string, VoteManager>(_legislator.BioguideId, _voteManager));

                getVotesTask.ContinueWith((antecedent) =>
                {
                    if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                        return;

                    Activity.RunOnUiThread(() =>
                    {
                        _votes = antecedent.Result;
                        ShowVotes(_votes);
                    });
                });
                getVotesTask.Start();
            }

            return fragment;
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (_votes != null)
            {
                var serializedVotes = _votes.SerializeToJson();
                outState.PutString(BundleType.Votes, serializedVotes);
            }
        }

        public void ShowVotes(List<Vote> votes)
        {
            (recyclerAdapter as VoteAdapter).UpdateVotes(votes);

            SetLoadingUiOff();
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