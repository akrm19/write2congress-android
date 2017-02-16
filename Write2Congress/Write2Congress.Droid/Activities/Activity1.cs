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

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "Activity1")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mainMenu_donate:
                    Toast.MakeText(ApplicationContext, "doonate",ToastLength.Short);
                    return true;

            }

            return base.OnOptionsItemSelected(item);
        }

        /*

                         public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            menu.Add(MenuId.PreferencesMenuGroupId, MenuId.PreferenceMenuExitApp, Menu.None, GetString(Resource.String.exitApp));

            return true;
        } 

         *         public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            if (item.ItemId == MenuId.PreferenceMenuExitApp)
            {
                using (var intent = new Intent(this, typeof(CloseApp)))
                {
                    intent.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(typeof(CloseApp));

                    Finish();
                    return true;
                }
            }

            
         * 

         */


    }
}