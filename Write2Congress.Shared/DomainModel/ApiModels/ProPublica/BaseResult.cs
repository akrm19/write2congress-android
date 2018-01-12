using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public abstract class BaseResult
    {
        public abstract class BaseRootObject
        {
            public string status { get; set; }
            public string copyright { get; set; }
            //public Result[] results { get; set; }
        }
    }
}
