using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Enum
{
    public enum BillStatusKind
    {
        Enacted, 
        AwaitingSignature,
        Vetoed, 
        InCongress,
        Unknown
    }
}
