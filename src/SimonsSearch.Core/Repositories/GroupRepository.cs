using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ISimonsSearchDataContext _dbContext;

        public GroupRepository(ISimonsSearchDataContext simonsSearchDataContext)
        {
            _dbContext = simonsSearchDataContext;
        }

        public IReadOnlyList<Group> GetGroupsMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Groups.Where(x =>
                    searchTerms.Any(s =>
                        (x.Name?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.Description?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false)))
                .ToList();
    }
}