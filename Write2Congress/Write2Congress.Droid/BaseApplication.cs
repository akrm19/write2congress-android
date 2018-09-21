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
using Write2Congress.Shared.DomainModel.Interface;

using Com.Instabug.Library;
using Com.Instabug.Library.Invocation;

namespace Write2Congress.Droid
{
    [Activity(Label = "BaseApplication")]
    public class BaseApplication : Application
    {
        private bool _forceRetrieveAllLegislators = false;
        private IMyLogger _logger;
        private static BaseApplication _instance;
        private List<Legislator> _allLegislators;
        private List<Legislator> _favoriteLegislators;

        public LetterManager LetterManager;
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
            new Instabug.Builder(this, "43f736535911298944ea7b86b88e6644")
                        .SetInvocationEvents(
                            //InstabugInvocationEvent.FloatingButton, 
                            InstabugInvocationEvent.Shake)
                        .SetPromptOptionsEnabled(false, true, true)
                        .Build();

            LetterManager = new LetterManager(new LetterFileProvider(), _logger);

            //CommitteeManager = new CommitteeManager(_logger);
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