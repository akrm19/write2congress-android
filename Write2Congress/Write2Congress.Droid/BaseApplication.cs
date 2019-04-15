using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Runtime;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Interface;

using Com.Instabug.Library;
using Com.Instabug.Library.Invocation;
using Com.Instabug.Bug;
using Write2Congress.Droid.DomainModel.Constants;

namespace Write2Congress.Droid
{
    [Activity(Label = "BaseApplication")]
    public class BaseApplication : Application
    {
        private bool _forceRetrieveAllLegislators = false;
        private string _instaBugNumber = "43f736535911298944ea7b86b88e6644";
        private IMyLogger _logger;
        private static BaseApplication _instance;
        private List<Legislator> _allLegislators;
        private List<Legislator> _favoriteLegislators;

        public CommitteeManager CommitteeManager;
        public VoteManager VoteMngr;
        public BillManager BillMngr; 
        protected LegislatorManager LegislatorManager;

        public List<Legislator> FavoriteLegislators 
        { 
            get => _favoriteLegislators ?? new List<Legislator>(); 

            set
            {
                if (value == null)
                    return;

                AppHelper.SaveFavoriteLegistorsToFileStorage(value);
				_favoriteLegislators = value; 
            }
        }

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
            _logger = new Logger("BaseApplication");

            //InstaBug init

            var enableShakeForFeedback = AppHelper.GetDefaultPreferenceBoolean(SharedPreference.EnableShareForFeedback, true);
            new Instabug.Builder(this, _instaBugNumber)
                        .SetInvocationEvents(enableShakeForFeedback
                            ? InstabugInvocationEvent.Shake
                            : InstabugInvocationEvent.None)
                        .Build();
            BugReporting.SetPromptOptionsEnabled(PromptOption.Feedback, PromptOption.Bug);
            Instabug.SetWelcomeMessageState(Com.Instabug.Library.UI.Onboarding.WelcomeMessage.State.Live);
            Instabug.PrimaryColor = AndroidHelper.GetCurrentSdkVer() < BuildVersionCodes.M
#pragma warning disable CS0618 // Type or member is obsolete
                ? Resources.GetColor(Resource.Color.primary_blue)
#pragma warning restore CS0618 // Type or member is obsolete
                : GetColor(Resource.Color.primary_blue);


			LegislatorManager = new LegislatorManager(_logger);
            VoteMngr = new VoteManager(_logger);
            BillMngr = new BillManager(_logger);

            _favoriteLegislators = AppHelper.GetFavoriteLegislatorsFromFileStorage();
            _allLegislators = AppHelper.GetCachedLegislatorsFromFileStorage();

            if (_allLegislators.Count == 0 || _forceRetrieveAllLegislators)
            {
                _allLegislators = LegislatorManager.GetAllLegislators();

                AppHelper.SaveCachedLegistorsToFileStorage(_allLegislators);
            }
        }

        public bool UpdateLegislatorData()
        {
            try
            {
                _allLegislators = LegislatorManager.GetAllLegislators();
                AppHelper.SaveCachedLegistorsToFileStorage(_allLegislators);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("An error occured updating legislator data", e);
                return false;
            }
        }

        public List<Legislator> GetCachedLegislators()
        {
            return _allLegislators;
        }
    }
}