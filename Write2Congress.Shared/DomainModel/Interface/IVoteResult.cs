using System;
using System.Collections.Generic;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IVoteResult
    {
        List<IVote> GetVoteResult();
    }
}
