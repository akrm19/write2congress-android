using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class SunlightCommitteeResult : SunlightBaseResult
    {
        public class Rootobject : BaseRootObject, ISunlightResult
        {
            public SunlightCommittee[] results { get; set; }
        }

        public class SunlightCommittee
        {
            public string chamber { get; set; }
            public string committee_id { get; set; }
            public string name { get; set; }
            public string office { get; set; }
            public string phone { get; set; }
            public string url { get; set; }
            public bool subcommittee { get; set; }
            public string parent_committee_id { get; set; }
        }
    }
}
