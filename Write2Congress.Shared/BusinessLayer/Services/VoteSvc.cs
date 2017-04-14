using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class VoteSvc : ServiceBase
    {
        public VoteSvc(IMyLogger logger)
        {
            SetLogger(logger);
        }


    }
}
