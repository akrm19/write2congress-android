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
using Write2Congress.Droid.Code;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Adapters;
using Android.Support.V7.Widget;

namespace Write2Congress.Droid.CustomControls
{
    public abstract class BaseViewer : LinearLayout
    {
        protected RecyclerView recycler;
        protected RecyclerView.Adapter recyclerAdapter;
        protected ViewSwitcher viewSwitcher;
        protected TextView header, emptyText;

        protected BaseFragment baseFragment;
        protected Logger myLogger;

        public BaseViewer(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BaseViewer(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        protected void Initialize()
        {
            myLogger = new Logger(Class.SimpleName);

            using (var layoutInflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater)
                layoutInflater.Inflate(Resource.Layout.ctrl_BaseViewer, this, true);
        }

        public virtual void SetupCtrl(BaseFragment fragment)
        {
            baseFragment = fragment;

            header = FindViewById<TextView>(Resource.Id.baseViewer_header);
            header.Text = ViewerTitle();

            viewSwitcher = FindViewById<ViewSwitcher>(Resource.Id.baseViewer_viewSwitcher);
            emptyText = FindViewById<TextView>(Resource.Id.baseViewer_emptyText);

            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            recycler = FindViewById<RecyclerView>(Resource.Id.baseViewer_recycler);
            recycler.SetLayoutManager(layoutManager);
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

        protected void ShowEmptyview()
        {
            if (viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
                viewSwitcher.ShowNext();
        }

        protected void ShowEmptyviewIfNecessary()
        {
            if (recyclerAdapter.ItemCount == 0 && viewSwitcher.NextView.Id == Resource.Id.baseViewer_emptyText)
                viewSwitcher.ShowNext();
            else if (recyclerAdapter.ItemCount > 0 && viewSwitcher.CurrentView.Id != Resource.Id.baseViewer_recycler)
                viewSwitcher.ShowNext();
        }

        protected abstract string EmptyText();

        protected abstract string ViewerTitle();
    }
}