using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public class GroupWeightCalculator : IGroupWeightCalculator
    {
        private const int NameWeight = 9;
        private const int NameTransitoryWeight = 8;
        private const int DescriptionWeight = 5;
        private const int FullMatchMultiplier = 10;

        public void CalculateWeights(IReadOnlyList<GroupDto> groups, string originalSearchQuery, IReadOnlyList<string> separateSearchTerms)
        {
            if (!(groups?.Any() ?? false))
            {
                return;
            }

            foreach (var groupDto in groups)
            {
                var fullyMatchedProperties = CalculateFullMatch(groupDto, originalSearchQuery);
                CalculatePartialMatches(groupDto, separateSearchTerms, fullyMatchedProperties);
            }
        }

        private static IReadOnlyList<string> CalculateFullMatch(GroupDto groupDto, string originalSearchQuery)
        {
            var fullyMatchedProperties = new List<string>();

            if (!string.IsNullOrWhiteSpace(groupDto.Name) && originalSearchQuery.Contains(groupDto.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                groupDto.Weight += NameWeight * FullMatchMultiplier;
                groupDto.TransitoryWeight += NameTransitoryWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(groupDto.Name));
            }

            if (!string.IsNullOrWhiteSpace(groupDto.Description) && originalSearchQuery.Contains(groupDto.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                groupDto.Weight += DescriptionWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(groupDto.Description));
            }

            return fullyMatchedProperties;
        }

        private static void CalculatePartialMatches(GroupDto groupDto, IReadOnlyList<string> separateSearchTerms, IReadOnlyList<string> fullyMatchedProperties)
        {
            foreach (var searchTerm in separateSearchTerms)
            {
                if (!fullyMatchedProperties.Contains(nameof(groupDto.Name)) &&
                    (groupDto.Name?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    groupDto.Weight += NameWeight;
                    groupDto.TransitoryWeight += NameTransitoryWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(groupDto.Description)) &&
                    (groupDto.Description?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    groupDto.Weight += DescriptionWeight;
                }
            }
        }
    }
}