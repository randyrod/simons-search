using System;
using System.Collections.Generic;
using System.Linq;
using SimonsSearch.Common.Enums;
using SimonsSearch.Core.Interfaces;
using SimonsSearch.Core.Models;
using SimonsSearch.Core.Repositories;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Services
{
    public class LockSearchService : ILockSearchService
    {
        private readonly ILockRepository _lockRepository;
        private readonly ILockWeightCalculator _lockWeightCalculator;

        public LockSearchService(ILockRepository lockRepository, ILockWeightCalculator lockWeightCalculator)
        {
            _lockRepository = lockRepository;
            _lockWeightCalculator = lockWeightCalculator;
        }

        public IReadOnlyList<LockDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms, IReadOnlyList<BuildingDto> buildings)
        {
            if (string.IsNullOrWhiteSpace(originalSearchQuery) || !(searchTerms?.Any() ?? false))
            {
                return new List<LockDto>();
            }

            var maybeLockTypes = TryGetLockTypesOnSearchTerms(searchTerms);

            var lockEntities = _lockRepository.GetLocksMatchingTerms(searchTerms, maybeLockTypes);

            if (!lockEntities.Any())
            {
                return new List<LockDto>();
            }

            var lockDtos = lockEntities.Select(ToLockDto).ToList();

            _lockWeightCalculator.CalculateWeights(lockDtos, buildings, originalSearchQuery, searchTerms);

            return lockDtos;
        }

        private static IReadOnlyList<LockType> TryGetLockTypesOnSearchTerms(IReadOnlyList<string> searchTerms)
        {
            var allowedLockTypes = Enum.GetNames(typeof(LockType)).ToList();
            var foundTerms = allowedLockTypes.Where(x => searchTerms.Any(s => x.Contains(s, StringComparison.InvariantCultureIgnoreCase))).ToList();

            var lockTypes = new List<LockType>();
            foreach (var e in foundTerms)
            {
                if (!Enum.TryParse(typeof(LockType), e, true, out var res) || res == null)
                {
                    continue;
                }

                lockTypes.Add((LockType) res);
            }

            return lockTypes;
        }

        private static LockDto ToLockDto(Lock lockEntity)
        => new LockDto
        {
            Id = lockEntity.Id,
            Name = lockEntity.Name,
            Description = lockEntity.Description,
            Floor = lockEntity.Floor,
            Type = lockEntity.Type,
            BuildingId = lockEntity.BuildingId,
            RoomNumber = lockEntity.RoomNumber,
            SerialNumber = lockEntity.SerialNumber
        };
    }
}