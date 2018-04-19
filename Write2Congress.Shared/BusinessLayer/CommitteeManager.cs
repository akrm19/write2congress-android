using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer
{
    public class CommitteeManager
    {
        //CommitteeSvc _committeeSvc;
        IMyLogger _logger; 

        public CommitteeManager(IMyLogger logger)
        {
            _logger = logger;
            //_committeeSvc = new CommitteeSvc(logger);
        }

        /*
        public List<Committee> GetCommitteesForLegislator(string legislatorBioguideId)
        {
            var committees = _committeeSvc.GetCommitteesForLegislator(legislatorBioguideId);

            return committees;
        }

        public bool IsThereMoreResultsForLastCall()
        {
            return _committeeSvc.IsThereMoreResults();
        }
        */
    }
}
