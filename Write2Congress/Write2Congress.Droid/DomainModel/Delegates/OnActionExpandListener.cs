﻿using System;
using Android.Views;

namespace Write2Congress.Droid.DomainModel.Delegates
{
    //Taken from https://gist.github.com/furi2/8796163
    public class OnActionExpandListener : Java.Lang.Object, Android.Support.V4.View.MenuItemCompat.IOnActionExpandListener
    {

        public class MenuItemEventArg : EventArgs
        {
            public IMenuItem MenuItem { get; set; }
            public bool Handled { get; set; }

            public MenuItemEventArg()
            {
                Handled = false;
            }
        }
        public delegate void MenuItemEventHandler(object sender, MenuItemEventArg e);

        public event MenuItemEventHandler MenuItemCollaspe;
        public event MenuItemEventHandler MenuItemActionExpand;

        public bool OnMenuItemActionCollapse(IMenuItem item)
        {
            if (MenuItemCollaspe != null)
            {
                MenuItemEventArg e = new MenuItemEventArg();
                e.MenuItem = item;
                MenuItemCollaspe(this, e);
                return e.Handled;
            }
            return true;
        }

        public bool OnMenuItemActionExpand(IMenuItem item)
        {
            if (MenuItemActionExpand != null)
            {
                MenuItemEventArg e = new MenuItemEventArg();
                e.MenuItem = item;
                MenuItemActionExpand(this, e);
                return e.Handled;
            }
            return true;
        }
    }
}
