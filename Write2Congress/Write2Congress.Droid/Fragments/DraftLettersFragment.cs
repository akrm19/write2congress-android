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
    public class DraftLettersFragment : BaseFragment
    {
        private LetterAdapter _adapter;
        private RecyclerView _draftsRecyclerView; 

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragment = inflater.Inflate(Resource.Layout.frag_DraftLetters, container, false);

            var toolbar = SetupToolbar(fragment, Resource.Id.draftLettersFrag_toolbar, AndroidHelper.GetString(Resource.String.drafts));

            _draftsRecyclerView = fragment.FindViewById<RecyclerView>(Resource.Id.draftLettersFrag_lettersRecycler);
            var layoutManager = new LinearLayoutManager(fragment.Context, LinearLayoutManager.Vertical, false);
            _draftsRecyclerView.SetLayoutManager(layoutManager);

            var letters = GetBaseApp().LetterManager.GetAllDraftLetters();
            _adapter = new LetterAdapter(this, letters);
            _draftsRecyclerView.SetAdapter(_adapter);

            return fragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }
    }
}