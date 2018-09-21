
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

using Com.Instabug.Bug;
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class FeedbackActivity : BaseToolbarActivity
    {
        public FeedbackActivity()
        {
        }

        protected override int DrawerLayoutId => Resource.Id.feedbackActv_parent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_Feedback);
            SetupToolbar(Resource.Id.feedbackActv_toolbar);//, GetString(Resource.String.feedback));
            SetupNavigationMenu(Resource.Id.feedbackActv_navigationDrawer);

            var feedbackButton = FindViewById(Resource.Id.feedbackActv_giveFeedback);
            feedbackButton.Click += FeedbackButton_Click;
        }

        protected override void OnStart()
        {
            base.OnStart();

            UpdateTitleBarText(AndroidHelper.GetString(Resource.String.feedback));
        }

        void FeedbackButton_Click(object sender, EventArgs e)
        {
            BugReporting.Invoke();
        }
    }
}
