using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services.APIs
{
    class UnitedStatesIoApi : ApiBase
    {
        public UnitedStatesIoApi(IMyLogger logger)
        {
            SetLogger(logger);
        }

        protected override string GetApiBaseUrl()
        {
            return "https://theunitedstates.io/";
        }

        protected override void ModifyHttpClientIfNeeded(HttpClient httpClient)
        {
            
        }
    }
}
