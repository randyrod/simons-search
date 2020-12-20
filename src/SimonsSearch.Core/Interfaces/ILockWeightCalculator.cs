using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface ILockWeightCalculator
    {
        void CalculateWeights(
            IReadOnlyList<LockDto> locks,
            IReadOnlyList<BuildingDto> buildings,
            string originalSearchTerms,
            IReadOnlyList<string> separateSearchTerms);
    }
}