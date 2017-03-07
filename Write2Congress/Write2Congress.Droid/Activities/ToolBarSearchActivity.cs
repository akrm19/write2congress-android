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
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Write2Congress.Droid.Activities
{
    public abstract class ToolBarSearchActivity : BaseToolbarActivity, ILegislatorViewerActivity
    {
        //New listener
        private SearchTextChangedDelegate _legislatorSearchTextChanged;

        protected override void SetupOnCreateOptionsMenu(IMenu menu)
        {
            SetupSearchButton(menu);
        }

        private void SetupSearchButton(IMenu menu)
        {
            using (var searchMenuitem = menu.FindItem(Resource.Id.mainMenu_search))
            using (var searchView = MenuItemCompat.GetActionView(searchMenuitem))
            using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                searchViewJavaObj.QueryTextChange += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.NewText);
                };

                searchViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    _legislatorSearchTextChanged?.Invoke(e.Query);
                    e.Handled = true;
                };
            }
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