using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly SimonsSearchDbContext _dbContext;

        public LockRepository(SimonsSearchDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<Lock> GetLocksMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Locks.Where(x => searchTerms.Any(s =>
                    x.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Description.Contains(s, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Floor.Contains(s, StringComparison.InvariantCultureIgnoreCase) ||
                    x.RoomNumber.Contains(s) ||
                    x.SerialNumber.Contains(s)))
                .Distinct()
                .ToList();
    }
}