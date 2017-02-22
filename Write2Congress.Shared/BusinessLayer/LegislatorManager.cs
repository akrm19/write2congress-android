using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public class LegislatorManager
    {
        LegislatorSvc _legislatorSvc;

        public LegislatorManager()
        {
            _legislatorSvc = new LegislatorSvc();
        }

        #region Get Legislator Methods
        public List<Legislator> GetLegislatorByState(State state)
        {
            return new List<Legislator>();

        }

        public List<Legislator> GetLegislatorByZipcode(string zipcode)
        {
            var legislators = new List<Legislator>();

            legislators = _legislatorSvc.GetLegislatorsByZipCode(zipcode).Result;

            return legislators;
        }
        #endregion
    }
}
