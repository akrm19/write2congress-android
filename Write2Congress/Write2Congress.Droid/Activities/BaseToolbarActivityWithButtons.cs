
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
using Write2Congress.Droid.DomainModel.Delegates;
using Write2Congress.Droid.DomainModel.Interfaces;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "BaseToolbarActivityWithButtons")]
    public abstract class BaseToolbarActivityWithButtons : BaseToolbarActivity
    {
        protected abstract int MenuItemId { get; }
        protected override int DrawerLayoutId => throw new NotImplementedException();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(MenuItemId, menu);

            return true;
        }

        protected void SetMenuItemsVisibility(int menuItemId, bool setAsVisible)
        {
            if (menuItemId != 0)
            {
                using (var toolbar = GetSupportToolbar())
                using (var menuItem = toolbar.Menu.FindItem(menuItemId))
                    menuItem.SetVisible(setAsVisible);
            }
        }
    }
}
