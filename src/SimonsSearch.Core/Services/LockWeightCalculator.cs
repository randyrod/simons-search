using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Extensions;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public class LockWeightCalculator : ILockWeightCalculator
    {
        private const int TypeWeight = 3;
        private const int NameWeight = 10;
        private const int SerialNumberWeight = 8;
        private const int FloorWeight = 6;
        private const int RoomWNumberWeight = 6;
        private const int DescriptionWeight = 6;
        private const int FullMatchMultiplier = 10;

        public void CalculateWeights(IReadOnlyList<LockDto> locks, IReadOnlyList<BuildingDto> buildings, string originalSearchTerms, IReadOnlyList<string> separateSearchTerms)
        {
            if (!(locks?.Any() ?? false))
            {
                return;
            }

            foreach (var lockDto in locks)
            {
                var fullyMatchedProperties = CalculateFullMatch(lockDto, originalSearchTerms);
                CalculateTypeWeight(lockDto, separateSearchTerms);
                CalculatePartialMatches(lockDto, separateSearchTerms, fullyMatchedProperties);
                lockDto.Weight += buildings.SingleOrDefault(x => x.Id == lockDto.BuildingId)?.TransitoryWeight ?? 0;
            }
        }

        private static IReadOnlyList<string> CalculateFullMatch(LockDto lockDto, string searchQuery)
        {
            var fullyMatchedProperties = new List<string>();

            if (searchQuery.ContainsInvariant(lockDto.Name))
            {
                lockDto.Weight += NameWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(lockDto.Name));
            }

            if (searchQuery.ContainsInvariant(lockDto.Description))
            {
                lockDto.Weight += DescriptionWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(lockDto.Description));
            }

            if (searchQuery.ContainsInvariant(lockDto.Floor))
            {
                lockDto.Weight += FloorWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(lockDto.Floor));
            }

            if (searchQuery.ContainsInvariant(lockDto.RoomNumber))
            {
                lockDto.Weight += RoomWNumberWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(lockDto.RoomNumber));
            }

            if (searchQuery.ContainsInvariant(lockDto.SerialNumber))
            {
                lockDto.Weight += SerialNumberWeight * FullMatchMultiplier;
                fullyMatchedProperties.Add(nameof(lockDto.SerialNumber));
            }

            return fullyMatchedProperties;
        }

        private static void CalculateTypeWeight(LockDto lockDto, IReadOnlyList<string> searchTerms)
        {
            var lockTypeString = lockDto.Type.ToString("G");

            if (searchTerms.Any(x => x.ContainsInvariant(lockTypeString)))
            {
                lockDto.Weight += TypeWeight * FullMatchMultiplier;
            }
            else if (searchTerms.Any(x => lockTypeString.ContainsInvariant(x)))
            {
                lockDto.Weight += TypeWeight;
            }
        }

        private static void CalculatePartialMatches(LockDto lockDto, IReadOnlyList<string> searchTerms, IReadOnlyList<string> fullyMatchedProperties)
        {
            foreach (var searchTerm in searchTerms)
            {
                if (!fullyMatchedProperties.Contains(nameof(lockDto.Name)) &&
                    (lockDto.Name?.ContainsInvariant(searchTerm) ?? false))
                {
                    lockDto.Weight += NameWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(lockDto.Description)) &&
                    (lockDto.Description?.ContainsInvariant(searchTerm) ?? false))
                {
                    lockDto.Weight += DescriptionWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(lockDto.Floor)) &&
                    (lockDto.Floor?.ContainsInvariant(searchTerm) ?? false))
                {
                    lockDto.Weight += FloorWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(lockDto.RoomNumber)) &&
                    (lockDto.RoomNumber?.ContainsInvariant(searchTerm) ?? false))
                {
                    lockDto.Weight += RoomWNumberWeight;
                }

                if (!fullyMatchedProperties.Contains(nameof(lockDto.SerialNumber)) &&
                    (lockDto.SerialNumber?.ContainsInvariant(searchTerm) ?? false))
                {
                    lockDto.Weight += SerialNumberWeight;
                }
            }
        }
    }
}