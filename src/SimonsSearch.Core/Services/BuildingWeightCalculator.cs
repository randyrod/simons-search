using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Extensions;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public class BuildingWeightCalculator : IBuildingWeightCalculator
    {
        private const int ShortCutWeight = 7;
        private const int ShortCutTransitoryWeight = 5;
        private const int NameWeight = 9;
        private const int NameTransitoryWeight = 8;
        private const int DescriptionWeight = 5;
        private const int FullMatchMultiplier = 10;

        public void CalculateWeights(IReadOnlyList<BuildingDto> buildings, string originalSearchQuery, IReadOnlyList<string> separateSearchTerms)
        {
            if (!(buildings?.Any() ?? false))
            {
                return;
            }

            foreach (var building in buildings)
            {
                var fullyMatchedProperties = CalculateFullMatch(building, originalSearchQuery);
                CalculatePartialMatches(building, separateSearchTerms, fullyMatchedProperties);
            }
        }

        private static IReadOnlyList<string> CalculateFullMatch(BuildingDto building, string searchQuery)
        {
            var fullyMatchedProperties = new List<string>();

            if (searchQuery.ContainsInvariant(building.Name))
            {
                building.Weight += NameWeight * FullMatchMultiplier;
                building.TransitoryWeight += NameTransitoryWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(building.Name));
            }

            if (searchQuery.ContainsInvariant(building.Description))
            {
                building.Weight += DescriptionWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(building.Description));
            }

            if (searchQuery.ContainsInvariant(building.ShortCut))
            {
                building.Weight += ShortCutWeight * FullMatchMultiplier;
                building.TransitoryWeight += ShortCutTransitoryWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(building.ShortCut));
            }

            return fullyMatchedProperties;
        }

        private static void CalculatePartialMatches(BuildingDto buildingDto, IReadOnlyList<string> searchTerms, IReadOnlyList<string> fullyMatched)
        {
            foreach (var searchTerm in searchTerms)
            {
                if (!fullyMatched.Contains(nameof(buildingDto.Name)) &&
                    (buildingDto.Name?.ContainsInvariant(searchTerm) ?? false))
                {
                    buildingDto.Weight += NameWeight;
                    buildingDto.TransitoryWeight += NameTransitoryWeight;
                }

                if (!fullyMatched.Contains(nameof(buildingDto.Description)) &&
                    (buildingDto.Description?.ContainsInvariant(searchTerm) ?? false))
                {
                    buildingDto.Weight += DescriptionWeight;
                }

                if (!fullyMatched.Contains(buildingDto.ShortCut) &&
                    (buildingDto.ShortCut?.ContainsInvariant(searchTerm) ?? false))
                {
                    buildingDto.Weight += ShortCutWeight;
                    buildingDto.TransitoryWeight += ShortCutTransitoryWeight;
                }
            }
        }
    }
}