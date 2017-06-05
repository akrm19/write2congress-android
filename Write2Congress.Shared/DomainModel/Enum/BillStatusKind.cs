using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Enum
{
    public enum BillStatusKind
    {
        [Description("Enacted")]
        Enacted,
        [Description("Awaiting Signature")]
        AwaitingSignature,
        [Description("Vetoed")]
        Vetoed,
        [Description("In Congress")]
        InCongress,
        [Description("Unknown")]
        Unknown
    }
}
