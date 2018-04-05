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

namespace Write2Congress.Droid.Fragments
{
    public abstract class BaseRecyclerViewerFragment : BaseFragment
    {
        protected RecyclerView recycler;
        protected RecyclerView.Adapter recyclerAdapter;
        protected ViewSwitcher viewSwitcher;
        protected TextView header, emptyText;
        protected LinearLayout recyclerButtonsParent;
        protected Button loadMoreButton;

        protected int currentPage = 1;
        protected bool IsBeingShown = false;
        private bool listenerSet = false;
        protected bool errorOccurred = false;
        public Action<bool> LoadMoreClick;

        protected BaseFragment baseFragment;

        public BaseRecyclerViewerFragment() { }

        protected virtual void CleanUp()
        {
            recycler = null;
            recyclerAdapter = null;
            viewSwitcher = null;
            header = null;
            emptyText = null;
            baseFragment = null;
            recyclerButtonsParent = null;

            if(loadMoreButton != null)
                loadMoreButton.Click -= NextButon_Click;

            loadMoreButton = null;
            LoadMoreClick = null;
        }

        protected virtual void HandleErrorRetrievingData()
        {
            errorOccurred = true;

            if (loadMoreButton != null)
                loadMoreButton.Text = AndroidHelper.GetString(Resource.String.tryAgain);

            if (emptyText != null)
                emptyText.Text = AndroidHelper.GetString(Resource.String.unableToRetrieveData);
        }

        protected virtual void HandleSuccessfullDataRetrieval()
        {
            errorOccurred = false;
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

            recyclerButtonsParent = fragment.FindViewById<LinearLayout>(Resource.Id.baseViewer_recyclerButtonsParent);

            loadMoreButton = fragment.FindViewById<Button>(Resource.Id.baseViewer_recyclerNextButton);
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
            {
                //currentPage = isNextClick
                //    ? currentPage + 1
                //    : currentPage;

                SetLoadMoreButtonTextAsLoading(true);
            }
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

            CleanUp();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            //CleanUp();
        }

        protected Legislator RetrieveLegislatorIfAvailable(Bundle savedInstanceState)
        {
            if (savedInstanceState == null)
                return null;

            var serialziedLegislator = Arguments.GetString(BundleType.Legislator);
            var legislator = new Legislator().DeserializeFromJson(serialziedLegislator);

            return legislator;
        }

        protected int RetrieveCurrentPageIfAvailable(Bundle savedInstanceState)
        {
            return savedInstanceState != null
                ? savedInstanceState.GetInt(BundleType.CurrentPage, 0) 
                : 0;
        }

        protected void SetLoadingUiOff()
        {
            emptyText.Text = EmptyText();
            ShowEmptyviewIfNecessary();
        }

        protected void SetLoadingUi()
        {
            emptyText.Text = AndroidHelper.GetString(Resource.String.loading);
            ShowEmptyview();
        }

        protected void ShowEmptyview(string textToShow = null)
        {
            if (!string.IsNullOrWhiteSpace(textToShow))
                emptyText.Text = textToShow;

            if (viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
                viewSwitcher.ShowNext();

            ShowRecyclerButtons(false);
            //SetLoadMoreButtonAsLoading(true);
        }

        protected void ShowEmptyviewIfNecessary()
        {
            if (recyclerAdapter.ItemCount == 0 && viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
            {
                viewSwitcher.ShowNext();
                //ShowRecyclerButtons(false);
            }
            else if (recyclerAdapter.ItemCount > 0 && viewSwitcher.CurrentView.Id != Resource.Id.baseViewer_recycler)
            {
                viewSwitcher.ShowNext();
                //ShowRecyclerButtons(true);
            }
        }

        protected void ShowRecyclerButtons(bool showButtons)
        {
            recyclerButtonsParent.Visibility =  showButtons
                ? ViewStates.Visible
                : ViewStates.Gone;

            loadMoreButton.Visibility = showButtons
                ? ViewStates.Visible
                : ViewStates.Gone;
        }

        public void SetLoadMoreButtonTextAsLoading(bool setAsLoading)
        {
            if (loadMoreButton == null)
                return;

            loadMoreButton.Text = AndroidHelper.GetString( setAsLoading
                ? Resource.String.loading
                : Resource.String.loadMore);

            loadMoreButton.Enabled = !setAsLoading;
        }

        protected abstract string EmptyText();

        public abstract string ViewerTitle();
    }
}