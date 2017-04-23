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
using Write2Congress.Droid.Adapters;
using Android.Support.V7.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Fragments
{
    public class BaseViewLetterFragment : BaseFragment
    {
        private LetterAdapter _adapter;
        private RecyclerView _lettersRecyclerView;
        private TextView _emptyDraftLettersText;
        private ViewSwitcher _viewSwitcher;
        private string _viewLettersFragmentType;

        public BaseViewLetterFragment()
        { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewLettersFragmentType = Arguments.GetString(BundleType.ViewLettersFragType, string.Empty);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;

            var fragment = inflater.Inflate(Resource.Layout.frag_ViewLetters, container, false);

            _viewSwitcher = fragment.FindViewById<ViewSwitcher>(Resource.Id.viewLettersFrag_viewSwitcher);

            _lettersRecyclerView = fragment.FindViewById<RecyclerView>(Resource.Id.viewLettersFrag_lettersRecycler);
            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            _lettersRecyclerView.SetLayoutManager(layoutManager);

            var letters = IsDraftLettersViewer()
                ?   GetBaseApp().LetterManager.GetAllDraftLetters()
                : GetBaseApp().LetterManager.GettAllSentLetters();

            _adapter = new LetterAdapter(this, letters);
            _adapter.LetterClick += OnLetterClicked;
            _adapter.CopyLetterSucceeded += LetterCopied;
            _adapter.DeleteLetterSucceeded += LetterDeleted;
            _lettersRecyclerView.SetAdapter(_adapter);

            _emptyDraftLettersText = fragment.FindViewById<TextView>(Resource.Id.viewLettersFrag_emptyText);
            _emptyDraftLettersText.Text = AndroidHelper.GetString(IsDraftLettersViewer()
                ? Resource.String.emptyDraftLettersText
                : Resource.String.emptySentLettersText);

            return fragment;
        }

        public override void OnResume()
        {
            base.OnResume();

            ShowEmptyviewIfNecessary();
        }

        private void ShowEmptyviewIfNecessary()
        {
            if (_adapter.ItemCount == 0 && _viewSwitcher.NextView.Id == _emptyDraftLettersText.Id)
                _viewSwitcher.ShowNext();
            else if (_adapter.ItemCount > 0 && _viewSwitcher.CurrentView.Id != _lettersRecyclerView.Id)
                _viewSwitcher.ShowNext();
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            GetToolbar().SetDisplayShowTitleEnabled(true);
            GetToolbar().Title = AndroidHelper.GetString( IsDraftLettersViewer()
                ? Resource.String.drafts
                : Resource.String.sent);

            base.OnPrepareOptionsMenu(menu);
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
                    RefreshLetters();
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            return true;
        }

        private void RefreshLetters()
        {
            var updatedLetters = IsDraftLettersViewer()
                ? GetBaseApp().LetterManager.GetAllDraftLetters()
                : GetBaseApp().LetterManager.GettAllSentLetters();

            _adapter.UpdateLetters(updatedLetters);
            ShowEmptyviewIfNecessary();
        }
        private void LetterDeleted(object sender, int e)
        {
            ShowEmptyviewIfNecessary();
        }

        private void LetterCopied(object sender, int e)
        {
            _lettersRecyclerView.SmoothScrollToPosition(0);
        }

        protected void OnLetterClicked(object sender, int position)
        {
            var letter = _adapter.GetLetterAtPosition(position);

            if (letter == null)
            {
                MyLogger.Error($"Unable to retrieve letter at position {position}");
                return;
            }

            AppHelper.StartWriteNewLetterIntent(GetBaseActivity(), BundleSenderKind.ViewLettersAdapter, letter, true);
        }

        private bool IsSentLettersViewer()
        {
            return _viewLettersFragmentType.Equals(ViewLettersFragmentType.Sent);
        }

        private bool IsDraftLettersViewer()
        {
            return _viewLettersFragmentType.Equals(ViewLettersFragmentType.Drafts);
        }
    }
}