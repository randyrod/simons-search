using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public interface ISearchService
    {
        IReadOnlyList<SearchResultDto> Search(string searchQuery);
    }
}