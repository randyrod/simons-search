using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly SimonsSearchDbContext _dbContext;

        public LockRepository()
        {
            _dbContext = new SimonsSearchDbContext();
        }

        public IReadOnlyList<Lock> GetLocksMatchingTerms(IReadOnlyList<string> searchTerms, IReadOnlyList<LockType> lockTypes) =>
            _dbContext.Locks.Where(x => searchTerms.Any(s =>
                    (x.Name?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.Description?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.Floor?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.RoomNumber?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.SerialNumber?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    lockTypes.Any(t => t == x.Type)))
                .Distinct()
                .ToList();
    }
}