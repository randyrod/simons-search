using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface IMediumSearchService
    {
        IReadOnlyList<MediumDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms, IReadOnlyList<GroupDto> groups);
    }
}