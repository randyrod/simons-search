using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Services
{
    public class GroupSearchService : IGroupSearchService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupWeightCalculator _groupWeightCalculator;

        public GroupSearchService(IGroupRepository groupRepository, IGroupWeightCalculator groupWeightCalculator)
        {
            _groupRepository = groupRepository;
            _groupWeightCalculator = groupWeightCalculator;
        }

        public IReadOnlyList<GroupDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms)
        {
            if (!(searchTerms?.Any() ?? false))
            {
                return new List<GroupDto>();
            }

            var groupEntities = _groupRepository.GetGroupsMatchingTerms(searchTerms);

            if (!groupEntities.Any())
            {
                return new List<GroupDto>();
            }

            var groupDtos = groupEntities.Select(ToGroupDto).ToList();

            _groupWeightCalculator.CalculateWeights(groupDtos, originalSearchQuery, searchTerms);

            return groupDtos;
        }

        private static GroupDto ToGroupDto(Group group)
            => new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description
            };
    }
}