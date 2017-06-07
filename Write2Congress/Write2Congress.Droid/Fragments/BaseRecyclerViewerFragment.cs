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

namespace Write2Congress.Droid.Fragments
{
    public abstract class BaseRecyclerViewerFragment : BaseFragment
    {
        protected RecyclerView recycler;
        protected RecyclerView.Adapter recyclerAdapter;
        protected ViewSwitcher viewSwitcher;
        protected TextView header, emptyText;
        protected LinearLayout recyclerButtonsParent;

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
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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

            using (var nextButon = fragment.FindViewById<Button>(Resource.Id.baseViewer_recyclerNextButton))
                nextButon.Click += NextButon_Click;

            return fragment;
        }

        protected virtual void NextButon_Click(object sender, EventArgs e) { }

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
        }

        protected void ShowEmptyviewIfNecessary()
        {
            if (recyclerAdapter.ItemCount == 0 && viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
            {
                viewSwitcher.ShowNext();
                ShowRecyclerButtons(false);
            }
            else if (recyclerAdapter.ItemCount > 0 && viewSwitcher.CurrentView.Id != Resource.Id.baseViewer_recycler)
            {
                viewSwitcher.ShowNext();
                ShowRecyclerButtons(true);
            }
        }

        protected void ShowRecyclerButtons(bool showButtons)
        {
            recyclerButtonsParent.Visibility =  showButtons
                ? ViewStates.Visible
                : ViewStates.Gone;
        }

        protected abstract string EmptyText();

        public abstract string ViewerTitle();
    }
}