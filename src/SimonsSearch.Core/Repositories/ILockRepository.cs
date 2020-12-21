using System.Collections.Generic;
using SimonsSearch.Common.Enums;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public interface ILockRepository
    {
        IReadOnlyList<Lock> GetLocksMatchingTerms(IReadOnlyList<string> searchTerms, IReadOnlyList<LockType> lockTypes);
    }
}