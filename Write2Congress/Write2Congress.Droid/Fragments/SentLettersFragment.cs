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

namespace Write2Congress.Droid.Fragments
{
    public class SentLettersFragment : BaseFragment
    {
        private LetterAdapter _adapter;
        private RecyclerView _sentLettersRecyclerView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = inflater.Inflate(Resource.Layout.frag_ViewLetters, container, false);

            var toolbar = SetupToolbar(fragment, Resource.Id.viewLettersFrag_toolbar, AndroidHelper.GetString(Resource.String.sent));
            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            _sentLettersRecyclerView = fragment.FindViewById<RecyclerView>(Resource.Id.viewLettersFrag_lettersRecycler);
            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            _sentLettersRecyclerView.SetLayoutManager(layoutManager);

            var letters = GetBaseApp().LetterManager.GettAllSentLetters();
            _adapter = new LetterAdapter(this, letters);
            _sentLettersRecyclerView.SetAdapter(_adapter);

            return fragment;
        }

        protected override void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.viewLettersMenu_refresh:
                    RefreshSentLetters();
                    break;
                case Resource.Id.viewLettersMenu_settings:
                    SettingsPressed();
                    break;
                case Resource.Id.viewLettersMenu_donate:
                    DonatePressed();
                    break;
                case Resource.Id.viewLettersMenu_exit:
                    ExitButtonPressed();
                    break;
                default:
                    base.Toolbar_MenuItemClick(sender, e);
                    break;
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_viewLetters, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        private void RefreshSentLetters()
        {
            var updatedLetters = GetBaseApp().LetterManager.GetAllDraftLetters();
            _adapter.UpdateLetters(updatedLetters);
        }
    }
}