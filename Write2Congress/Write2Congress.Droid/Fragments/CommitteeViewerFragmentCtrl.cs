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
        private CommitteeManager _committeeManager;
        private List<Committee> _committees;
        private Legislator _legislator;

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

            _committeeManager = new CommitteeManager(MyLogger);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new CommitteeAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();

            if (_committees != null && _committees.Count > 0)
                ShowCommittees(_committees);
            else if (savedInstanceState != null && !string.IsNullOrWhiteSpace(savedInstanceState.GetString(BundleType.Committees, string.Empty)))
            {
                var serializedCommittees = savedInstanceState.GetString(BundleType.Committees);
                _committees = new List<Committee>().DeserializeFromJson(serializedCommittees);
                ShowCommittees(_committees);
            }
            else
            {
                var getCommitteesTask = new Task<List<Committee>>((prms) =>
                {
                    var legislatorId = (prms as Tuple<string, CommitteeManager>).Item1;
                    var cm = (prms as Tuple<string, CommitteeManager>).Item2;
                    return cm.GetCommitteesForLegislator(legislatorId);
                },new Tuple<string, CommitteeManager>(_legislator.BioguideId, _committeeManager));

                getCommitteesTask.ContinueWith((antecedent) =>
                {
                    if (Activity == null || Activity.IsDestroyed || Activity.IsFinishing)
                        return;

                    Activity.RunOnUiThread(() =>
                    {
                        _committees = antecedent.Result;
                        ShowCommittees(_committees);
                    });
                });
                getCommitteesTask.Start();
            }

            return fragment;
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

        public void ShowCommittees(List<Committee> committees)
        {
            (recyclerAdapter as CommitteeAdapter).UpdateCommittee(committees);

            SetLoadingUiOff();
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyCommitteesText);
        }

        public override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.committees);
        }
    }
}