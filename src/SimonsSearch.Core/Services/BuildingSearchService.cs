using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Services
{
    public class BuildingSearchService : IBuildingSearchService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IBuildingWeightCalculator _buildingWeightCalculator;

        public BuildingSearchService(IBuildingRepository buildingRepository, IBuildingWeightCalculator buildingWeightCalculator)
        {
            _buildingRepository = buildingRepository;
            _buildingWeightCalculator = buildingWeightCalculator;
        }

        public IReadOnlyList<BuildingDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms)
        {
            if (!(searchTerms?.Any() ?? false))
            {
                return new List<BuildingDto>();
            }

            var buildingEntities = _buildingRepository.GetBuildingsMatchingTerms(searchTerms);

            if (!buildingEntities.Any())
            {
                return new List<BuildingDto>();
            }

            var buildingDtos = buildingEntities.Select(ToBuildingDto).ToList();

            _buildingWeightCalculator.CalculateWeights(buildingDtos, originalSearchQuery, searchTerms);

            return buildingDtos;
        }

        private static BuildingDto ToBuildingDto(Building building) =>
            new BuildingDto
            {
                Id = building.Id,
                Description = building.Description,
                Name = building.Name,
                ShortCut = building.ShortCut
            };
    }
}