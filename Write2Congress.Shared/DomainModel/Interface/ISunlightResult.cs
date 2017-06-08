using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Write2Congress.Shared.DomainModel.SunlightBaseResult;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface ISunlightResult
    {
        int? count { get; set; }
        Page page { get; set; }
    }
}
