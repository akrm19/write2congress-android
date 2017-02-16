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

namespace Write2Congress.Droid
{
    [Activity(Label = "BaseApplication")]
    public class BaseApplication : Application
    {
        private static BaseApplication _instance;

        public BaseApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            _instance = this;
        }

        public BaseApplication()
        {
            _instance = this;
        }
    }
}