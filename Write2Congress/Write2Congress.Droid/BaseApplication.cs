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
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid
{
    [Activity(Label = "BaseApplication")]
    public class BaseApplication : Application
    {
        private static BaseApplication _instance;
        private List<Legislator> _allLegislators;
        protected LegislatorManager LegislatorManager; 

        public BaseApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            _instance = this;
        }

        public BaseApplication()
        {
            _instance = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            LegislatorManager = new LegislatorManager();
            _allLegislators = AppHelper.GetCachedLegislatorsFromFileStorage();

            if (_allLegislators.Count == 0)
            {
                _allLegislators = LegislatorManager.GetAllLegislators();

                AppHelper.SaveLegistorsToFileStorage(_allLegislators);
            }
        }

        public List<Legislator> GetCachedLegislators()
        {
            return _allLegislators;
        }
    }
}