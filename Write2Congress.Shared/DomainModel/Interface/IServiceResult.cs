using System;
using System.Collections.Generic;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IServiceResult<T>
    {
        List<T> GetResults();
    }
}
