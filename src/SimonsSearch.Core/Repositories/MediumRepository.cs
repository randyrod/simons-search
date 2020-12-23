using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Enums;
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

        public IReadOnlyList<Medium> GetMediumsMatchingTerms(IReadOnlyList<string> searchTerms, IReadOnlyList<MediumType> mediumTypes) =>
            _dbContext.Mediums.Where(x =>
                    searchTerms.Any(s =>
                        (x.Owner?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.Description?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.SerialNumber?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        mediumTypes.Any(t => t == x.Type)))
                .ToList();
    }
}