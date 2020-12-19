using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly SimonsSearchDbContext _dbContext;

        public BuildingRepository(SimonsSearchDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IReadOnlyList<Building> GetBuildingsMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Buildings.Where(x =>
                    searchTerms.Any(s =>
                        x.Name.Contains(s) ||
                        x.Description.Contains(s) ||
                        x.ShortCut.Contains(s)))
                .Distinct()
                .ToList();
    }
}