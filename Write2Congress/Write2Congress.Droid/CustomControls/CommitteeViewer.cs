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
    public class CommitteeViewer : LinearLayout
    {
        private CommitteeAdapter _committeeAdapter;
        private ViewSwitcher _viewSwitcher;
        private TextView _header, _emptyText;

        private CommitteeManager _committeeManager; 
        private List<Committee> _committees;
        private BaseFragment _fragment;
        protected Logger _logger;

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

        private void Initialize()
        {
            _logger = new Logger(Class.SimpleName);
            _committeeManager = new CommitteeManager(_logger);

            using (var layoutInflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater)
                layoutInflater.Inflate(Resource.Layout.ctrl_CommitteeViewer, this, true);
        }

        public void SetupCtrl(BaseFragment fragment)
        {
            _fragment = fragment;

            _viewSwitcher = FindViewById<ViewSwitcher>(Resource.Id.committeeViewer_viewSwitcher);
            _header = FindViewById<TextView>(Resource.Id.committeeViewer_header);
            _emptyText = FindViewById<TextView>(Resource.Id.committeeViewer_emptyText);

            var layoutManager = new LinearLayoutManager(_fragment.Context, LinearLayoutManager.Vertical, false);
            var recyclerView = FindViewById<RecyclerView>(Resource.Id.committeeViewer_recycler);
            recyclerView.SetLayoutManager(layoutManager);

            _committeeAdapter = new CommitteeAdapter(_fragment);
            recyclerView.SetAdapter(_committeeAdapter);
        }

        public void ShowLegislatorCommittees(Legislator legislator)
        {
            SetLoadingUi();

            //TODO RM:Make async task
            var committees = _committeeManager.GetCommitteesForLegislator(legislator.BioguideId);
            _committeeAdapter.UpdateCommittee(committees);
            
            SetLoadingUiOff();
        }

        private void SetLoadingUiOff()
        {
            _emptyText.Text = AndroidHelper.GetString(Resource.String.emptyCommitteesText);
            ShowEmptyviewIfNecessary();
        }

        private void SetLoadingUi()
        {
            _emptyText.Text = AndroidHelper.GetString(Resource.String.loading);
            ShowEmptyview();
        }

        private void ShowEmptyview()
        {
            if (_viewSwitcher.NextView.Id == Resource.Id.committeeViewer_emptyText)
                _viewSwitcher.ShowNext();
        }

        private void ShowEmptyviewIfNecessary()
        {
            if (_committeeAdapter.ItemCount == 0 && _viewSwitcher.NextView.Id == Resource.Id.committeeViewer_emptyText)
                _viewSwitcher.ShowNext();
            else if (_committeeAdapter.ItemCount > 0 && _viewSwitcher.CurrentView.Id != Resource.Id.committeeViewer_recycler)
                _viewSwitcher.ShowNext();
        }
    }
}