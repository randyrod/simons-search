using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public class MediumWeightCalculator : IMediumWeightCalculator
    {
        private const int TypeWeight = 3;
        private const int OwnerWeight = 10;
        private const int SerialNumberWeight = 8;
        private const int DescriptionWeight = 6;
        private const int FullMatchMultiplier = 10;

        public void CalculateWeights(IReadOnlyList<MediumDto> mediums, IReadOnlyList<GroupDto> groups, string originalSearchTerms, IReadOnlyList<string> separateSearchTerms)
        {
            if (!(mediums?.Any() ?? false))
            {
                return;
            }

            foreach (var mediumDto in mediums)
            {
                var fullyMatchedProperties = CalculateFullMatch(mediumDto, originalSearchTerms);
                CalculatePartialMatches(mediumDto, separateSearchTerms, fullyMatchedProperties);
                CalculateTypeWeight(mediumDto, separateSearchTerms);
                mediumDto.Weight += groups.FirstOrDefault(x => x.Id == mediumDto.GroupId)?.TransitoryWeight ?? 0;
            }
        }

        private static IReadOnlyList<string> CalculateFullMatch(MediumDto mediumDto, string searchQuery)
        {
            var fullyMatchedProperties = new List<string>();

            if (!string.IsNullOrWhiteSpace(mediumDto.Owner) &&
                searchQuery.Contains(mediumDto.Owner, StringComparison.InvariantCultureIgnoreCase))
            {
                mediumDto.Weight += OwnerWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(mediumDto.Owner));
            }

            if (!string.IsNullOrWhiteSpace(mediumDto.Description) && searchQuery.Contains(mediumDto.Description,
                StringComparison.InvariantCultureIgnoreCase))
            {
                mediumDto.Weight += DescriptionWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(mediumDto.Description));
            }

            if (!string.IsNullOrWhiteSpace(mediumDto.SerialNumber) && searchQuery.Contains(mediumDto.SerialNumber,
                StringComparison.InvariantCultureIgnoreCase))
            {
                mediumDto.Weight += SerialNumberWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(mediumDto.SerialNumber));
            }

            return fullyMatchedProperties;
        }

        private static void CalculatePartialMatches(MediumDto mediumDto, IReadOnlyList<string> separateSearchTerms, IReadOnlyList<string> matchedProperties)
        {
            foreach (var searchTerm in separateSearchTerms)
            {
                if (!matchedProperties.Contains(nameof(mediumDto.Description)) &&
                    (mediumDto.Description?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    mediumDto.Weight += DescriptionWeight;
                }

                if (!matchedProperties.Contains(nameof(mediumDto.Owner)) &&
                    (mediumDto.Owner?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    mediumDto.Weight += OwnerWeight;
                }

                if (!matchedProperties.Contains(nameof(mediumDto.SerialNumber)) &&
                    (mediumDto.SerialNumber?.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ??
                     false))
                {
                    mediumDto.Weight += SerialNumberWeight;
                }
            }
        }

        private static void CalculateTypeWeight(MediumDto mediumDto, IReadOnlyList<string> searchTerms)
        {
            var mediumTypeString = mediumDto.Type.ToString("G");

            if (searchTerms.Any(x => string.Equals(x, mediumTypeString, StringComparison.InvariantCultureIgnoreCase)))
            {
                mediumDto.Weight += TypeWeight * FullMatchMultiplier;
            }
            else if (searchTerms.Any(x => mediumTypeString.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
            {
                mediumDto.Weight += TypeWeight;
            }
        }
    }
}