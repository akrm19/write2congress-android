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
using Android.Support.V7.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.BusinessLayer;
using Android.Support.Design.Widget;

namespace Write2Congress.Droid.Fragments
{
    public abstract class BaseRecyclerViewerFragment : BaseFragment
    {
        protected RecyclerView recycler;
        protected ViewSwitcher viewSwitcher;
        protected TextView header, emptyText;
        protected FloatingActionButton loadMoreButton;

        protected int currentPage = 1;
        protected bool IsBeingShown = false;
        private bool listenerSet = false;
        protected bool errorOccurred = false;
        public Action<bool> LoadMoreClick;

        public BaseRecyclerViewerFragment() { }

        protected virtual void CleanUpReferencesToViewOrContext()
        {
            recycler?.GetAdapter()?.Dispose();
            recycler?.Dispose();// = null;
            //recyclerAdapter = null;
            viewSwitcher = null;
            header = null;
            emptyText = null;

            if(loadMoreButton != null)
                loadMoreButton.Click -= NextButon_Click;

            loadMoreButton = null;
            LoadMoreClick = null;
        }

        protected virtual void HandleErrorRetrievingData()
        {
            errorOccurred = true;

            if (loadMoreButton != null)
                loadMoreButton.Enabled = true;

            if (emptyText != null)
                emptyText.Text = AndroidHelper.GetString(Resource.String.unableToRetrieveData);

            ShowToast(AndroidHelper.GetString(Resource.String.dataRetrievedError), ToastLength.Long);
        }


        protected virtual void HandleSuccessfullDataRetrieval()
        {
			errorOccurred = false;

            if (loadMoreButton != null)
                loadMoreButton.Enabled = true;
			
            //Do not show toast the first time we retrieve data
            if(currentPage > 1)
                ShowToast(GetSuccessfullDataRetrievalMessage(), ToastLength.Long);
        }

        public override void OnResume()
        {
            base.OnResume();

            IsBeingShown = true;
        }

        public override void OnPause()
        {
            base.OnPause();

            IsBeingShown = false;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = inflater.Inflate(Resource.Layout.ctrl_BaseViewer, container, false);

            header = fragment.FindViewById<TextView>(Resource.Id.baseViewer_header);
            //header.Text = ViewerTitle();
            header.Visibility = ViewStates.Gone;

            viewSwitcher = fragment.FindViewById<ViewSwitcher>(Resource.Id.baseViewer_viewSwitcher);
            emptyText = fragment.FindViewById<TextView>(Resource.Id.baseViewer_emptyText);

            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            recycler = fragment.FindViewById<RecyclerView>(Resource.Id.baseViewer_recycler);
            recycler.SetLayoutManager(layoutManager);

            loadMoreButton = fragment.FindViewById<FloatingActionButton>(Resource.Id.baseViewer_recyclerNextButton);
            loadMoreButton.Click += NextButon_Click;

            return fragment;
        }

        protected virtual void NextButon_Click(object sender, EventArgs e)
        {
            FetchMoreLegislatorContent(true);
        }

        protected virtual void FetchMoreLegislatorContent(bool isNextClick)
        {
            if (isNextClick)
                SetLoadMoreButtonInDisabledState(true);
        }

        public void SetOnClickListener(Action<bool> listener)
        {
            if (!listenerSet)
                LoadMoreClick = listener;

            listenerSet = true;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt(BundleType.CurrentPage, currentPage);
        }

        /// <summary>
        /// Fragments in the FragmentPagerAdapter are only detached 
        /// and never removed from the FragmentManager (unless the 
        /// Activity is finished). When using FragmentPagerAdapter 
        /// you must make sure to clear any references to the current 
        /// View or Context in onDestroyView()
        /// </summary>
        public override void OnDestroyView()
        {
            base.OnDestroyView();

            CleanUpReferencesToViewOrContext();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected Legislator RetrieveLegislatorIfAvailable(Bundle savedInstanceState)
        {
            if (savedInstanceState == null)
                return null;

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);

            return string.IsNullOrWhiteSpace(serialziedLegislator)
                         ? null
                         : new Legislator().DeserializeFromJson(serialziedLegislator);
        }

        protected int RetrieveCurrentPageIfAvailable(Bundle savedInstanceState)
        {
            return savedInstanceState != null
                ? savedInstanceState.GetInt(BundleType.CurrentPage, 1) 
                : 1;
        }

        protected void SetLoadingUiOff()
        {
            emptyText.Text = EmptyText();
            ShowEmptyviewIfNecessary();
        }

        protected void SetLoadingTextInEmptyView()
        {
            ShowEmptyview(AndroidHelper.GetString(Resource.String.loading));
        }

        protected void ShowEmptyview(string textToShow = null)
        {
            if (!string.IsNullOrWhiteSpace(textToShow))
                emptyText.Text = textToShow;

            if (viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
                viewSwitcher.ShowNext();

            SetLoadMoreButtonVisibility(false);
        }

        protected void ShowEmptyviewIfNecessary()
        {
            //if (recyclerAdapter.ItemCount == 0 && viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
            if(recycler.GetAdapter().ItemCount == 0 && viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
            {
                viewSwitcher.ShowNext();
                //ShowRecyclerButtons(false);
            }
            //else if (recyclerAdapter.ItemCount > 0 && viewSwitcher.CurrentView.Id != Resource.Id.baseViewer_recycler)
            else if (recycler.GetAdapter().ItemCount > 0 && viewSwitcher.CurrentView.Id != Resource.Id.baseViewer_recycler)
            {
                viewSwitcher.ShowNext();
                //ShowRecyclerButtons(true);
            }
        }

        protected void SetLoadMoreButtonVisibility(bool showButtons)
        {
            loadMoreButton.Visibility = showButtons
                ? ViewStates.Visible
                : ViewStates.Gone;
        }

        public void SetLoadMoreButtonInDisabledState(bool setAsLoading)
        {
            if (loadMoreButton == null)
                return;

            /*
            loadMoreButton.Text = AndroidHelper.GetString( setAsLoading
                ? Resource.String.loading
                : Resource.String.loadMore);
            */
            loadMoreButton.Enabled = !setAsLoading;

            //TODO RM: looking into chaing icon to loading gif
            loadMoreButton.Background.SetAlpha(setAsLoading
                                               ? 100
                                               : 255);
        }

        protected virtual string GetSuccessfullDataRetrievalMessage()
        {
            return AndroidHelper.GetString(Resource.String.dataRetrievedSuccessfully);
        }

        protected abstract string EmptyText();

        public abstract string ViewerTitle();
    }
}