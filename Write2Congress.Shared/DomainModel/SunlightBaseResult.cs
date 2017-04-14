using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public abstract class SunlightBaseResult
    {
        public class BaseRootobject
        {
            //public object[] results { get; set; }
            public int? count { get; set; }
            public Page page { get; set; }
        }

        public class Page
        {
            public int? count { get; set; }
            public int? per_page { get; set; }
            public int? page { get; set; }
        }
    }
}
