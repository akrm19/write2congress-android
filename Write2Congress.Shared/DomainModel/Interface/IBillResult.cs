using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IBillResult
    {
        List<IBill> GeBillResult();
    }
}
