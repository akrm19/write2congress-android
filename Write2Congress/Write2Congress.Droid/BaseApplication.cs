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

namespace Write2Congress.Droid
{
    [Activity(Label = "BaseApplication")]
    public class BaseApplication : Application
    {
        private bool _forceRetrieveAllLegislators = false;
        private IMyLogger _logger;
        private static BaseApplication _instance;
        private List<Legislator> _allLegislators;

        public LetterManager LetterManager;
        public CommitteeManager CommitteeManager;
        public VoteManager VoteMngr;
        public BillManager BillMngr; 
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
            _logger = new Logger("BaseApplication");

            LetterManager = new LetterManager(new LetterFileProvider());

            CommitteeManager = new CommitteeManager(_logger);
            VoteMngr = new VoteManager(_logger);
            BillMngr = new BillManager(_logger);
            LegislatorManager = new LegislatorManager();
            _allLegislators = AppHelper.GetCachedLegislatorsFromFileStorage();

            if (_allLegislators.Count == 0 || _forceRetrieveAllLegislators)
            {
                _allLegislators = LegislatorManager.GetAllLegislators();

                AppHelper.SaveLegistorsToFileStorage(_allLegislators);
            }
        }

        public bool UpdateLegislatorData()
        {
            try
            {
                _allLegislators = LegislatorManager.GetAllLegislators();
                AppHelper.SaveLegistorsToFileStorage(_allLegislators);

                return true;
            }
            catch (Exception e)
            {
                //TODO RM: Add logging
                return false;
            }
        }

        public List<Legislator> GetCachedLegislators()
        {
            return _allLegislators;
        }
    }
}