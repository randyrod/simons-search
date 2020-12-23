using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBuildingSearchService _buildingSearchService;
        private readonly IGroupSearchService _groupSearchService;
        private readonly ILockSearchService _lockSearchService;
        private readonly IMediumSearchService _mediumSearchService;

        public SearchService(IBuildingSearchService buildingSearchService, ILockSearchService lockSearchService, IGroupSearchService groupSearchService, IMediumSearchService mediumSearchService)
        {
            _buildingSearchService = buildingSearchService;
            _lockSearchService = lockSearchService;
            _groupSearchService = groupSearchService;
            _mediumSearchService = mediumSearchService;
        }

        public IReadOnlyList<SearchResultDto> Search(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<SearchResultDto>();
            }

            searchQuery = searchQuery.ToUpperInvariant();

            var separateSearchTerms = GetSeparateTerms(searchQuery);

            var buildings = _buildingSearchService.Search(searchQuery, separateSearchTerms);
            var locks = _lockSearchService.Search(searchQuery, separateSearchTerms, buildings);
            var groups = _groupSearchService.Search(searchQuery, separateSearchTerms);
            var mediums = _mediumSearchService.Search(searchQuery, separateSearchTerms, groups);

            var searchResults = buildings
                .Select(b => new SearchResultDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Metadata = b.ToString(),
                    Type = ResultType.Building,
                    Weight = b.Weight
                }).ToList();

            searchResults
                .AddRange(locks.Select(l => new SearchResultDto
                        {
                            Id = l.Id,
                            Name = l.Name,
                            Metadata = l.ToString(),
                            Type = ResultType.Lock,
                            Weight = l.Weight
                        }));

            searchResults.AddRange(groups.Select(g => new SearchResultDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Metadata = g.ToString(),
                    Type = ResultType.Group,
                    Weight = g.Weight
                }));

            searchResults.AddRange(mediums.Select(m => new SearchResultDto
            {
                Id = m.Id,
                Name = m.Owner,
                Metadata = m.ToString(),
                Type = ResultType.Medium,
                Weight = m.Weight
            }));

            var result = searchResults
                .OrderByDescending(x => x.Weight)
                .ToList();

            return result;
        }

        private static IReadOnlyList<string> GetSeparateTerms(string searchTerms) => searchTerms.Split(' ').ToList();
    }
}