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
        [Description("passage")]
        Passage,
        [Description("cloture")]
        Cloture,
        [Description("nomination")]
        Nomination,
        [Description("impeachment")]
        Impeachment,
        [Description("treaty")]
        Treaty,
        [Description("recommit")]
        Recommit,
        [Description("quorum")]
        Quorum,
        [Description("leadership")]
        Leadership,
        [Description("other")]
        Other
    }
}
