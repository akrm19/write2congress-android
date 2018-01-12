using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class ServiceBase
    {
        protected IMyLogger _logger;

        public ServiceBase(IMyLogger logger)
        {
            _logger = logger;
        }
    }
}
