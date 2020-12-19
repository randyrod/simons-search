using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public interface IBuildingRepository
    {
        IReadOnlyList<Building> GetBuildingsMatchingTerms(IReadOnlyList<string> searchTerms);
    }
}