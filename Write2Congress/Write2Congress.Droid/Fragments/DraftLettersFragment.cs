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
using Write2Congress.Droid.Adapters;
using Android.Support.V7.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.Activities;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Enums;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Write2Congress.Droid.Fragments
{
    public class DraftLettersFragment : BaseFragment
    {
        private LetterAdapter _adapter;
        private RecyclerView _draftsRecyclerView; 

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;

            var fragment = inflater.Inflate(Resource.Layout.frag_ViewLetters, container, false);

            _draftsRecyclerView = fragment.FindViewById<RecyclerView>(Resource.Id.viewLettersFrag_lettersRecycler);
            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            _draftsRecyclerView.SetLayoutManager(layoutManager);

            var letters = GetBaseApp().LetterManager.GetAllDraftLetters();
            _adapter = new LetterAdapter(this, letters);
            _adapter.LetterClick += OnLetterClicked;
            _adapter.CopyLetterSucceeded += CopyLetterSucceeded;
            _draftsRecyclerView.SetAdapter(_adapter);

            return fragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        private void CopyLetterSucceeded(object sender, int e)
        {
            _draftsRecyclerView.SmoothScrollToPosition(0);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_viewLetters, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.viewLettersMenu_refresh:
                    RefreshDraftLetters();
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            return true;
        }

        private void RefreshDraftLetters()
        {
            var updatedLetters = GetBaseApp().LetterManager.GetAllDraftLetters();
            _adapter.UpdateLetters(updatedLetters);
        }

        protected void OnLetterClicked(object sender, int position)
        {
            var letter = _adapter.GetLetterAtPosition(position);

            if(letter == null)
            {
                MyLogger.Error($"Unable to retrieve letter at position {position}");
                return;
            }

            AppHelper.StartWriteNewLetterIntent(GetBaseActivity(), BundleSenderKind.ViewLettersAdapter, letter, true);
        }
    }
}