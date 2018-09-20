
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
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Delegates;
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "BaseToolbarActivityWithSearch")]
    public abstract class BaseToolbarActivityWithSearch : BaseToolbarActivityWithButtons, IActivityWithToolbarSearch
    {
        protected ToolbarMenuItemClickedDelegate _exitSearchClicked;
        protected FilterDataTextChangedDelegate _filterDataTextChanged;
        protected FilterDataTextChangedDelegate _searchTextChanged;
        protected ToolbarMenuItemClickedDelegate _filterSearchviewCollapsed;
        protected ToolbarMenuItemClickedDelegate _searchSearchviewCollapsed;

        protected virtual int FilterDataItemId => 0;
        protected virtual int SearchItemId =>  0;
        protected virtual int ExitSearchItemId => 0;

        protected string CurrentSearch;
        protected string CurrentFilter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null)
            {
                if (savedInstanceState.ContainsKey(BundleType.CurrentSearchQuery))
                    CurrentSearch = savedInstanceState.GetString(BundleType.CurrentSearchQuery);

                if (savedInstanceState.ContainsKey(BundleType.CurrentFilterQuery))
                    CurrentFilter = savedInstanceState.GetString(BundleType.CurrentFilterQuery);
            }

            if (FilterDataItemId != 0)
            {
                _filterDataTextChanged += (string newValue) =>
                {
                    CurrentFilter = newValue;
                };

                _filterSearchviewCollapsed += () =>
                {
                    CurrentFilter = string.Empty;
                };
            }

            if (SearchItemId != 0)
            {
                _searchTextChanged += (string newValue) =>
                {
                    CurrentSearch = newValue;
                };

                _exitSearchClicked += () =>
                {
                    CurrentSearch = string.Empty;
                };
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(MenuItemId, menu);

            SetupFilterMenuItem(menu);
            SetupSearchMenuItem(menu);

            return true;
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            if (CurrentSearch != null)
                outState.PutString(BundleType.CurrentSearchQuery, CurrentSearch);

            if (CurrentFilter != null)
                outState.PutString(BundleType.CurrentFilterQuery, CurrentFilter);
        }

        protected override void OnDestroy()
        {
            _filterDataTextChanged = null;
            _searchTextChanged = null;
            _exitSearchClicked = null;
            _filterSearchviewCollapsed = null;
            _searchSearchviewCollapsed = null;

            base.OnDestroy();
        }

        public virtual void ClearFilterTextChangedDelegate()
        {
            _exitSearchClicked = null;
            _searchTextChanged = null;
			_filterDataTextChanged = null;
            _filterSearchviewCollapsed = null;
            _searchSearchviewCollapsed = null;
        }

        public void CollapseToolbarSearchview()
        {
            if (SearchItemId != 0)
            {
                using (var toolbar = GetSupportToolbar())
                using (var menuItem = toolbar.Menu.FindItem(SearchItemId))
                    menuItem.CollapseActionView();
            }            
        }

        protected  virtual void SetupFilterMenuItem(IMenu menu)
        {
            if (FilterDataItemId != 0)
            {
                using (var filterMenuItem = menu.FindItem(FilterDataItemId))
                using (var filterView = MenuItemCompat.GetActionView(filterMenuItem))
                using (var filterViewJavaObj = filterView.JavaCast<Android.Support.V7.Widget.SearchView>())
                {
                    filterViewJavaObj.QueryHint = AndroidHelper.GetString(Resource.String.enterFilterCriteria);
                    filterViewJavaObj.QueryTextChange += (s, e) =>
                    {
                        _filterDataTextChanged?.Invoke(e.NewText);
                        e.Handled = true;
                    };

                    filterViewJavaObj.QueryTextSubmit += (s, e) =>
                    {
                        _filterDataTextChanged?.Invoke(e.Query);
                        e.Handled = true;
                    };

                    var onCollapseListener = new OnActionExpandListener();
                    onCollapseListener.MenuItemCollaspe += (sender, e) =>
                    {
                        _filterSearchviewCollapsed?.Invoke();
                        e.Handled = true;
                    };

                    Android.Support.V4.View.MenuItemCompat.SetOnActionExpandListener(filterMenuItem, onCollapseListener);

                    if (!string.IsNullOrWhiteSpace(CurrentFilter))
                    {
                        var previousFilter = CurrentFilter;

                        //For some reason these to invoke QueryTextChanged
                        // the secodn time it is called, the CurrentFilter is set to empty
                        filterMenuItem.ExpandActionView();

                        CurrentFilter = previousFilter;
                        filterViewJavaObj.SetQuery(CurrentFilter, false);

                        CurrentFilter = previousFilter;
                    }
                }
            }
        }

        protected virtual void SetupSearchMenuItem(IMenu menu)
        {
            if (SearchItemId != 0)
            {
                using (var searchMenuItem = menu.FindItem(SearchItemId))
                using (var searchView = MenuItemCompat.GetActionView(searchMenuItem))
                using (var searchViewJavaObj = searchView.JavaCast<Android.Support.V7.Widget.SearchView>())
                {
                    searchViewJavaObj.QueryHint = AndroidHelper.GetString(Resource.String.enterSearchCriteria);
                    searchViewJavaObj.QueryTextSubmit += (s, e) =>
                    {
                        _searchTextChanged?.Invoke(e.Query);
                        e.Handled = true;
                    };

                    var onSearchCollapsedListener = new OnActionExpandListener();
                    onSearchCollapsedListener.MenuItemCollaspe += (sender, e) =>
                        {
                            _searchSearchviewCollapsed?.Invoke();
                            e.Handled = true;
                        };

                    MenuItemCompat.SetOnActionExpandListener(searchMenuItem, onSearchCollapsedListener);
                }
            }
        }

        public void SetToolbarExitSearchviewVisibility(bool setAsVisible)
        {
            SetMenuItemsVisibility(ExitSearchItemId, setAsVisible);
        }

		public void SetToolbarSearchviewVisibility(bool setAsVisible)
		{
			SetMenuItemsVisibility(SearchItemId, setAsVisible);
		}

        public void SetToolbarFilterviewVisibility(bool setAsVisible)
        {
            SetMenuItemsVisibility(FilterDataItemId, setAsVisible);
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

        public virtual FilterDataTextChangedDelegate SearchQuerySubmitted
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

        public virtual ToolbarMenuItemClickedDelegate ExitSearchClicked
        {
            get
            {
                return _exitSearchClicked;
            }
            set
            {
                _exitSearchClicked += value;
            }
        }


        public virtual ToolbarMenuItemClickedDelegate FilterSearchviewCollapsed
        {
            get => _filterSearchviewCollapsed;

            set
            {
                _filterSearchviewCollapsed += value;
            }
        }

        public virtual ToolbarMenuItemClickedDelegate SearchSearchviewCollapsed
        {
            get => _searchSearchviewCollapsed; 

            set
            {
                _searchSearchviewCollapsed += value;
            }
        }
    }
}
