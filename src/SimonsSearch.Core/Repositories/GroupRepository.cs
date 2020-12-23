using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Data;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly SimonsSearchDbContext _dbContext;

        public GroupRepository(SimonsSearchDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadOnlyList<Group> GetGroupsMatchingTerms(IReadOnlyList<string> searchTerms) =>
            _dbContext.Groups.Where(x =>
                    searchTerms.Any(s =>
                        (x.Name?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                        (x.Description?.Contains(s, StringComparison.InvariantCultureIgnoreCase) ?? false)))
                .ToList();
    }
}