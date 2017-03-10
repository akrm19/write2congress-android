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
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "BaseToolbarActivity")]
    public abstract class BaseToolbarActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            SetupOnCreateOptionsMenu(menu);

            return base.OnCreateOptionsMenu(menu);
        }

        protected abstract void SetupOnCreateOptionsMenu(IMenu menu);

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_writeNew:
                    AppHelper.GetWriteNewLetterIntent(this);
                    return true;
                case Resource.Id.mainMenu_settings:
                    SettingsPressed();
                    return true;
                case Resource.Id.mainMenu_donate:
                    DonatePressed();
                    return true;
                case Resource.Id.mainMenu_exit:
                    ExitButtonPressed();
                    return true;
                default:
                    return true;
            }
            //return base.OnOptionsItemSelected(item);
        }

        public void ExitButtonPressed()
        {
            FinishAffinity();
        }

        public void DonatePressed()
        {

        }

        public void SettingsPressed()
        {

        }
    }


}