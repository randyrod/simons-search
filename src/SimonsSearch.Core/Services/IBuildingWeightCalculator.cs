using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Services
{
    public interface IBuildingWeightCalculator
    {
        void CalculateWeights(IReadOnlyList<BuildingDto> buildings, string originalSearchQuery, IReadOnlyList<string> separateSearchTerms);
    }
}