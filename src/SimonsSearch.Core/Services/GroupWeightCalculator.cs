using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Extensions;
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

            if (originalSearchQuery.ContainsInvariant(groupDto.Name))
            {
                groupDto.Weight += NameWeight * FullMatchMultiplier;
                groupDto.TransitoryWeight += NameTransitoryWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(groupDto.Name));
            }

            if (originalSearchQuery.ContainsInvariant(groupDto.Description))
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
                    (groupDto.Name?.ContainsInvariant(searchTerm) ?? false))
                {
                    groupDto.Weight += NameWeight;
                    groupDto.TransitoryWeight += NameTransitoryWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(groupDto.Description)) &&
                    (groupDto.Description?.ContainsInvariant(searchTerm) ?? false))
                {
                    groupDto.Weight += DescriptionWeight;
                }
            }
        }
    }
}