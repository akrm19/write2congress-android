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
    public class BillManager
    {
        private BillSvc _billSvc;
        private int _defautlResultsPerPage = 40;

        public  BillManager(IMyLogger logger)
        {
            _billSvc = new BillSvc(logger);
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page)
        {
            return GetBillsSponsoredbyLegislator(legislatorBioguideId, page, _defautlResultsPerPage);
        }

        public List<Bill> GetBillsSponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (resultsPerPage < 1)
                resultsPerPage = _defautlResultsPerPage;

            return _billSvc.GetBillsSponsoredbyLegislator(legislatorBioguideId, page, resultsPerPage);
        }

        public List<Bill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page)
        {
            return GetBillsCosponsoredbyLegislator(legislatorBioguideId, page, _defautlResultsPerPage);
        }

        public List<Bill> GetBillsCosponsoredbyLegislator(string legislatorBioguideId, int page, int resultsPerPage)
        {
            if (resultsPerPage < 1)
                resultsPerPage = _defautlResultsPerPage;

            return _billSvc.GetBillsCosponsoredbyLegislator(legislatorBioguideId, page, resultsPerPage);
        }
    }
}
