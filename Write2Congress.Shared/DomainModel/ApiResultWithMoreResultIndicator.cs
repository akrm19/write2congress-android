using System;
using System.Collections.Generic;

namespace Write2Congress.Shared.DomainModel
{
    public class ApiResultWithMoreResultIndicator<T>
    {
        public ApiResultWithMoreResultIndicator(List<T> results, bool isThereMoreVotes)
        {
            Results = results;
            IsThereMoreResults = isThereMoreVotes;
        }

        public bool IsThereMoreResults;
        public List<T> Results;
    }
}
