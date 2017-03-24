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
            HasOptionsMenu = true;

            var fragment = inflater.Inflate(Resource.Layout.frag_ViewLetters, container, false);

            _sentLettersRecyclerView = fragment.FindViewById<RecyclerView>(Resource.Id.viewLettersFrag_lettersRecycler);
            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            _sentLettersRecyclerView.SetLayoutManager(layoutManager);

            var letters = GetBaseApp().LetterManager.GettAllSentLetters();
            _adapter = new LetterAdapter(this, letters);
            _sentLettersRecyclerView.SetAdapter(_adapter);

            return fragment;
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
                    RefreshSentLetters();
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            return true;
        }

        private void RefreshSentLetters()
        {
            var updatedLetters = GetBaseApp().LetterManager.GetAllDraftLetters();
            _adapter.UpdateLetters(updatedLetters);
        }
    }
}