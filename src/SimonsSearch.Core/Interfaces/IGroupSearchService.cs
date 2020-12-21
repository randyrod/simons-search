using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface IGroupSearchService
    {
        IReadOnlyList<GroupDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms);
    }
}