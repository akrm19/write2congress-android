
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
        protected FilterDataTextChangedDelegate _filterDataTextChanged;

		protected abstract int MenuItemId { get; }
		protected abstract int FilterDataItemId { get; }		
        protected override int DrawerLayoutId => throw new NotImplementedException();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(MenuItemId, menu);

            using (var filterMenuItem = menu.FindItem(FilterDataItemId))
            using (var filterView = MenuItemCompat.GetActionView(filterMenuItem))
            using (var filterViewJavaObj = filterView.JavaCast<Android.Support.V7.Widget.SearchView>())
            {
                filterViewJavaObj.QueryHint = AndroidHelper.GetString(Resource.String.enterFilterCriteria);

                filterViewJavaObj.QueryTextChange += (s, e) =>
                {
                    _filterDataTextChanged?.Invoke(e.NewText);
                };

                filterViewJavaObj.QueryTextSubmit += (s, e) =>
                {
                    _filterDataTextChanged?.Invoke(e.Query);
                    e.Handled = true;
                };
            }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            _filterDataTextChanged = null;

            base.OnDestroy();
        }

        public virtual void ClearFilterTextChangedDelegate()
        {
            _filterDataTextChanged = null;
        }

        public virtual FilterDataTextChangedDelegate FilterSearchTextChanged
        {
            get
            {
                return _filterDataTextChanged;
            }
            set
            {
                _filterDataTextChanged += value;
            }
        }
    }
}
