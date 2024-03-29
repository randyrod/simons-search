using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ISimonsSearchDataContext _dbContext;

        public BuildingRepository(ISimonsSearchDataContext simonsSearchDataContext)
        {
            _dbContext = simonsSearchDataContext;
        }

        public IReadOnlyList<Building> GetBuildingsMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Buildings.Where(x =>
                    searchTerms.Any(s =>
                        (x.Name?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.Description?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.ShortCut?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false)))
                .Distinct()
                .ToList();
    }
}