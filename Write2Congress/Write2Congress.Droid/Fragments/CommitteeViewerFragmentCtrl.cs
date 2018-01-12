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

        public CommitteeViewerFragmentCtrl() {}

        public static CommitteeViewerFragmentCtrl CreateInstance(Legislator legislator)
        {
            var newFragment = new CommitteeViewerFragmentCtrl();

            //var args = new Bundle();
            //args.PutString(BundleType.Legislator, legislator.SerializeToJson());
            //newFragment.Arguments = args;

            return newFragment;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_committees == null)
                SetLoadingUi();
            else
                ShowCommittees(_committees);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = base.OnCreateView(inflater, container, savedInstanceState);

            recyclerAdapter = new CommitteeAdapter(this);
            recycler.SetAdapter(recyclerAdapter);

            //We load all committees, so no need for more buttons
            recyclerButtonsParent.Visibility = ViewStates.Gone;
            loadMoreButton.Visibility = ViewStates.Gone;

            if (_committees == null)
                SetLoadingUi();
            else
                SetCommittees(_committees);

            return fragment;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            //if(_committees != null)
            //{
            //    var serializedCommittees = _committees.SerializeToJson();
            //    outState.PutString(BundleType.Committees, serializedCommittees);
            //}
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _committees = null;
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

            if (IsBeingShown)
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