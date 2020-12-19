using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;

namespace SimonsSearch.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ILockRepository _lockRepository;
        private readonly IMediumRepository _mediumRepository;

        public SearchService(IBuildingRepository buildingRepository, IGroupRepository groupRepository, ILockRepository lockRepository, IMediumRepository mediumRepository)
        {
            _buildingRepository = buildingRepository;
            _groupRepository = groupRepository;
            _lockRepository = lockRepository;
            _mediumRepository = mediumRepository;
        }

        public IReadOnlyList<SearchResultDto> Search(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<SearchResultDto>();
            }

            var separateSearchTerms = GetSeparateTerms(searchQuery);

            var buildings = _buildingRepository.GetBuildingsMatchingTerms(separateSearchTerms);
            var locks = _lockRepository.GetLocksMatchingTerms(separateSearchTerms);
            var groups = _groupRepository.GetGroupsMatchingTerms(separateSearchTerms);
            var mediums = _mediumRepository.GetMediumsMatchingTerms(separateSearchTerms);

            var searchResults = new List<SearchResultDto>();

            foreach (var building in buildings)
            {
                searchResults.Add(new SearchResultDto
                {
                    Name = building.Name,
                    Metadata = string.Join(", ", building.Description, building.ShortCut),
                    Type = ResultType.Building
                });
            }

            foreach (var sLock in locks)
            {
                searchResults.Add(new SearchResultDto
                {
                    Name = sLock.Name,
                    Metadata = string.Join(", ", sLock.Description, sLock.Floor, sLock.Type.ToString("G"), sLock.RoomNumber, sLock.SerialNumber),
                    Type = ResultType.Lock
                });
            }

            foreach (var group in groups)
            {
                searchResults.Add(new SearchResultDto
                {
                    Name = group.Name,
                    Metadata = group.Description,
                    Type = ResultType.Group
                });
            }

            foreach (var medium in mediums)
            {
                searchResults.Add(new SearchResultDto
                {
                    Name = medium.Owner,
                    Metadata = string.Join(", ", medium.Description, medium.SerialNumber, medium.Type.ToString("G")),
                    Type = ResultType.Medium
                });
            }

            return searchResults;
        }

        private static IReadOnlyList<string> GetSeparateTerms(string searchTerms)
        {
            searchTerms = searchTerms.ToUpperInvariant();

            return searchTerms.Split(' ').ToList();
        }
    }
}