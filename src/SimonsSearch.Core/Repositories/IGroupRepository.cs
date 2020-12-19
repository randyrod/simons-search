using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public interface IGroupRepository
    {
        IReadOnlyList<Group> GetGroupsMatchingTerms(IReadOnlyList<string> searchTerms);
    }
}