using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class MediumRepository : IMediumRepository
    {
        private readonly SimonsSearchDbContext _dbContext;

        public MediumRepository(SimonsSearchDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<Medium> GetMediumsMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Mediums.Where(x =>
                    searchTerms.Any(s =>
                        x.Owner.Contains(s, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Description.Contains(s, StringComparison.InvariantCultureIgnoreCase) ||
                        x.SerialNumber.Contains(s, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
    }
}