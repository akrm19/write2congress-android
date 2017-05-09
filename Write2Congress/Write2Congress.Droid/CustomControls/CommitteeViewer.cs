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
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.Adapters;
using Android.Util;
using Android.Support.V7.Widget;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid.CustomControls
{
    public class CommitteeViewer : BaseViewer
    {
        private CommitteeManager _committeeManager; 
        private List<Committee> _committees;

        public CommitteeViewer(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public CommitteeViewer(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public override void SetupCtrl(BaseFragment fragment)
        {
            base.SetupCtrl(fragment);

            _committeeManager = new CommitteeManager(myLogger);

            recyclerAdapter = new CommitteeAdapter(fragment);
            recycler.SetAdapter(recyclerAdapter);

            SetLoadingUi();
        }

        public void ShowLegislatorCommittees(Legislator legislator)
        {
            SetLoadingUi();

            //TODO RM:Make async task
            var committees = _committeeManager.GetCommitteesForLegislator(legislator.BioguideId);
            (recyclerAdapter as CommitteeAdapter).UpdateCommittee(committees);
            
            SetLoadingUiOff();
        }

        protected override string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.emptyCommitteesText);
        }

        protected override string ViewerTitle()
        {
            return AndroidHelper.GetString(Resource.String.committees);
        }
    }
}