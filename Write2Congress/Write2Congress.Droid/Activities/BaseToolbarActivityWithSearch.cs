
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Write2Congress.Droid.Code;
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "BaseToolbarActivityWithSearch")]
    public abstract class BaseToolbarActivityWithSearch : BaseToolbarActivity, IActivityWithToolbarSearch
    {
        protected SearchTextChangedDelegate _searchTextChanged;

		protected abstract int MenuItemId { get; }
		protected abstract int SearchItemId { get; }		
        protected override int DrawerLayoutId => throw new NotImplementedException();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(MenuItemId, menu);

            using (var searchMenuitem = menu.FindItem(SearchItemId))
            using (var searchView = MenuItemCompat.GetActionView(searchMenuitem))
            using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                searchViewJavaObj.QueryHint = AndroidHelper.GetString(Resource.String.enterFilterCriteria);

                searchViewJavaObj.QueryTextChange += (s, e) =>
                {
                    //TODO RM: Convert _legislatorSearchTextChanged in 
                    //MainActivity into _searchTextChanged
                    _searchTextChanged?.Invoke(e.NewText);
                };

                searchViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    _searchTextChanged?.Invoke(e.Query);
                    e.Handled = true;
                };
            }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            _searchTextChanged = null;

            base.OnDestroy();
        }

        public virtual void ClearLegislatorSearchTextChangedDelegate()
        {
            _searchTextChanged = null;
        }

        public virtual SearchTextChangedDelegate LegislatorSearchTextChanged
        {
            get
            {
                return _searchTextChanged;
            }
            set
            {
                _searchTextChanged += value;
            }
        }
    }
}
