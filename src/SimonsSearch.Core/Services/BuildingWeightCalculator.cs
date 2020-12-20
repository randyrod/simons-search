using System;
using System.Collections.Generic;
using System.Linq;
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

            if (string.Equals(building.Name, searchQuery, StringComparison.InvariantCultureIgnoreCase))
            {
                building.Weight += NameWeight * FullMatchMultiplier;
                building.TransitoryWeight += NameTransitoryWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(building.Name));
            }

            if (string.Equals(building.Description, searchQuery, StringComparison.InvariantCultureIgnoreCase))
            {
                building.Weight += DescriptionWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(building.Description));
            }

            if (string.Equals(building.ShortCut, searchQuery, StringComparison.InvariantCultureIgnoreCase))
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
                    (buildingDto.Name?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    buildingDto.Weight += NameWeight;
                    buildingDto.TransitoryWeight += NameTransitoryWeight;
                }

                if (!fullyMatched.Contains(nameof(buildingDto.Description)) &&
                    (buildingDto.Description?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    buildingDto.Weight += DescriptionWeight;
                }

                if (!fullyMatched.Contains(buildingDto.ShortCut) &&
                    (buildingDto.ShortCut?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    buildingDto.Weight += ShortCutWeight;
                    buildingDto.TransitoryWeight += ShortCutTransitoryWeight;
                }
            }
        }
    }
}