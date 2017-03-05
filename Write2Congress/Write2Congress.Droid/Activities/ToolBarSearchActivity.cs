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
using Write2Congress.Droid.Interfaces;
using Android.Support.V4.View;

namespace Write2Congress.Droid.Activities
{
    public class ToolBarSearchActivity : BaseActivity, ILegislatorViewerActivity
    {
        //New listener
        private SearchTextChangedDelegate _legislatorSearchTextChanged;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            using (var searchMenuitem = menu.FindItem(Resource.Id.mainMenu_search))
            using (var searchView = MenuItemCompat.GetActionView(searchMenuitem))
            using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                searchViewJavaObj.QueryTextChange += (s, e) =>
                {
                    Toast.MakeText(this, e.NewText, ToastLength.Short).Show();

                    _legislatorSearchTextChanged?.Invoke(e.NewText);
                };

                searchViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    Toast.MakeText(this, "Search Query Submitted: " + e.Query, ToastLength.Long).Show();

                    _legislatorSearchTextChanged?.Invoke(e.Query);

                    e.Handled = true;
                };
            }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            _legislatorSearchTextChanged = null;

            base.OnDestroy();
        }

        public SearchTextChangedDelegate LegislatorSearchTextChanged
        {
            get
            {
                return _legislatorSearchTextChanged;
            }
            set
            {
                _legislatorSearchTextChanged += value;
            }
        }

        public void ClearLegislatorSearchTextChangedDelegate()
        {
            _legislatorSearchTextChanged = null;
        }
    }
}