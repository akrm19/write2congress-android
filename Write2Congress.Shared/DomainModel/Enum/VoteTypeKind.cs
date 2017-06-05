using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Enum
{
    public enum VoteTypeKind
    {
        [Description("Passage")]
        Passage,
        [Description("Cloture")]
        Cloture,
        [Description("Nomination")]
        Nomination,
        [Description("Impeachment")]
        Impeachment,
        [Description("Treaty")]
        Treaty,
        [Description("Recommit")]
        Recommit,
        [Description("Quorum")]
        Quorum,
        [Description("Leadership")]
        Leadership,
        [Description("Other")]
        Other
    }
}
