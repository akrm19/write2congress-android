using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class BillType
    {
        public BillType(BillTypeKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }

        public BillTypeKind Kind;
        public string Value;
    }
}
