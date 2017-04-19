using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Enum
{
    public enum VoteCastedType
    {
        [Description("Nay")]
        Nay,
        [Description("Yea")]
        Yea,
        [Description("Not Voting")]
        NotVoting,
        [Description("Present")]
        Present,
        [Description("Unknown")]
        Unknown
    }
}
