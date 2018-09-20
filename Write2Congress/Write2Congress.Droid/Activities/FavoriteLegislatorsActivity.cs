
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
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "FavoriteBillsActivity")]
    public class FavoriteLegislatorsActivity : BaseToolbarActivityWithButtons
    {
        private FavoriteLegislatorsFragment _favLegislatorsFragment;

        protected override int DrawerLayoutId => Resource.Id.favoriteLegislatorsActv_parent;

        protected override int MenuItemId => Resource.Menu.menu_favorite;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actv_FavoriteLegislators);

            SetupToolbar(Resource.Id.favoriteLegislatorsActv_toolbar);
            SetupNavigationMenu(Resource.Id.favoriteLegislatorsActv_navigationDrawer);


            _favLegislatorsFragment = SupportFragmentManager.FindFragmentByTag(TagsType.FavorityLegislatorsFragment) as FavoriteLegislatorsFragment;

            if (_favLegislatorsFragment == null)
            {
                _favLegislatorsFragment = new FavoriteLegislatorsFragment();
                AndroidHelper.AddSupportFragment(SupportFragmentManager, _favLegislatorsFragment, Resource.Id.favoriteLegislatorsActv_fragmentContainer, TagsType.FavorityLegislatorsFragment);
            }
                
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //case Resource.Id.favoriteMenu_edit:
                    ////EditFavoriteLegislators();
                    //return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
