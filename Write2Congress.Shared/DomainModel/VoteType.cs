using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class VoteType
    {
        public VoteTypeKind Type;
        public string Value;

        public VoteType(string value, VoteTypeKind voteType)
        {
            Value = value;
            Type = voteType;
        }
    }
}
