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
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Adapters;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using System.Threading.Tasks;

namespace Write2Congress.Droid.Fragments
{
    public class CommitteeViewerFragmentCtrl : BaseRecyclerViewerFragment
    {
        private List<Committee> _committees;
        private Legislator _legislator;
        private LegislatorManager _legislatorManager;

        public CommitteeViewerFragmentCtrl() {}

        public static CommitteeViewerFragmentCtrl CreateInstance(Legislator legislator)
        {
            var newFragment = new CommitteeViewerFragmentCtrl();

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

            _legislatorManager = new LegislatorManager(MyLogger);
            currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            currentPage = RetrieveCurrentPageIfAvailable(savedInstanceState);

            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new CommitteeAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();

            //We load all committees, so no need for more buttons
            recyclerButtonsParent.Visibility = ViewStates.Gone;
            loadMoreButton.Visibility = ViewStates.Gone;

            if (_committees != null && _committees.Count() >= 0)
                SetCommittees(_committees);
            else if(savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Committees, string.Empty)))
            {
                var serializedCommittees = savedInstanceState.GetString(BundleType.Committees);
                _committees = new List<Committee>().DeserializeFromJson(serializedCommittees);
                SetCommittees(_committees);
            }
            else
                FetchMoreLegislatorContent(false);

            return fragment;
        }

        protected override void FetchMoreLegislatorContent(bool isNextClick)
        {
            base.FetchMoreLegislatorContent(isNextClick);

            var getVotesTask = new Task<Tuple<List<Committee>>>((prms) =>
            {
                var passedParams = prms as Tuple<string, LegislatorManager>;

                var legislatorId = passedParams.Item1;
                var lm = passedParams.Item2;

                var results = lm.GetLegislatorsCommittees(legislatorId);

                var resultsAsICommittee = new List<Committee>();

                foreach (var c in results)
                    resultsAsICommittee.Add(Committee.FromICommittee(c));

                return new Tuple<List<Committee>>(resultsAsICommittee);
            }, new Tuple<string, LegislatorManager>(_legislator.IdBioguide, _legislatorManager));

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

                        //currentPage = antecedent.Result.Item3 + 1;

                        if (_committees == null || !_committees.Any())
                            _committees = antecedent.Result.Item1;
                        else
                            _committees.AddRange(antecedent.Result.Item1);

                        ShowCommittees(_committees);
                    }
                });
            });

            getVotesTask.Start();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if(_committees != null)
            {
                var serializedCommittees = _committees.SerializeToJson();
                outState.PutString(BundleType.Committees, serializedCommittees);
            }
        }

        public override void OnResume()
        {
            base.OnResume();

            if (errorOccurred)
                HandleErrorRetrievingData();
            else if (_committees == null)
                SetLoadingUi();
            else
                ShowCommittees(_committees);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _committees = null;
            _legislatorManager = null;
            _legislator = null;
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyCommitteesText);
        }

        public override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.committees);
        }

        public void SetCommittees(List<Committee> committees)
        {
            _committees = committees;
        }

        public void ShowCommittees(List<Committee> committees)
        {
            _committees = committees;

            if (IsBeingShown && recyclerAdapter != null)
            {
                (recyclerAdapter as CommitteeAdapter).UpdateCommittee(_committees);

                SetLoadingUiOff();
            }
        }

        protected override void NextButon_Click(object sender, EventArgs e)
        {

        }
    }
}