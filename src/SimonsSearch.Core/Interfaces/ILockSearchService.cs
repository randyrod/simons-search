using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface ILockSearchService
    {
        IReadOnlyList<LockDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms, IReadOnlyList<BuildingDto> buildings);
    }
}