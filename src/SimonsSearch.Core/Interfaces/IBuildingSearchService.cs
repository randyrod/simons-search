using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface IBuildingSearchService
    {
        IReadOnlyList<BuildingDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms);
    }
}