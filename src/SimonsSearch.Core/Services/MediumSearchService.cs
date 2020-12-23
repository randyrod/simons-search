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
    public class MediumSearchService : IMediumSearchService
    {
        private readonly IMediumRepository _mediumRepository;
        private readonly IMediumWeightCalculator _mediumWeightCalculator;

        public MediumSearchService(IMediumRepository mediumRepository, IMediumWeightCalculator mediumWeightCalculator)
        {
            _mediumRepository = mediumRepository;
            _mediumWeightCalculator = mediumWeightCalculator;
        }

        public IReadOnlyList<MediumDto> Search(string originalSearchQuery, IReadOnlyList<string> searchTerms, IReadOnlyList<GroupDto> groups)
        {
            if (!(searchTerms?.Any() ?? false))
            {
                return new List<MediumDto>();
            }

            var maybeMediumTypes = TryGetMediumTypesOnSearchTerms(searchTerms);

            var mediumEntities = _mediumRepository.GetMediumsMatchingTerms(searchTerms, maybeMediumTypes);

            if (!mediumEntities.Any())
            {
                return new List<MediumDto>();
            }

            var mediumDtos = mediumEntities.Select(ToMediumDto).ToList();

            _mediumWeightCalculator.CalculateWeights(mediumDtos, groups, originalSearchQuery, searchTerms);

            return mediumDtos;
        }

        private static IReadOnlyList<MediumType> TryGetMediumTypesOnSearchTerms(IReadOnlyList<string> searchTerms)
        {
            var allowedMediumTypes = Enum.GetNames(typeof(MediumType)).ToList();
            var foundTerms = allowedMediumTypes
                .Where(x => searchTerms.Any(s => x.Contains(s, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            var mediumTypes = new List<MediumType>();

            foreach (var term in foundTerms)
            {
                if (!Enum.TryParse(typeof(MediumType), term, true, out var result) || result == null)
                {
                    continue;
                }

                mediumTypes.Add((MediumType) result);
            }

            return mediumTypes;
        }

        private static MediumDto ToMediumDto(Medium medium) =>
            new MediumDto
            {
                Id = medium.Id,
                Owner = medium.Owner,
                Description = medium.Description,
                Type = medium.Type,
                GroupId = medium.GroupId,
                SerialNumber = medium.SerialNumber
            };
    }
}