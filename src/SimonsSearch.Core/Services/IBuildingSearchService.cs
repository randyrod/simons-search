using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public interface IBuildingSearchService
    {
        IReadOnlyList<BuildingDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms);
    }
}