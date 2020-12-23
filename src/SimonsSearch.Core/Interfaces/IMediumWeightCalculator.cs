using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface IMediumWeightCalculator
    {
        void CalculateWeights(IReadOnlyList<MediumDto> mediums, IReadOnlyList<GroupDto> groups, string originalSearchTerms, IReadOnlyList<string> separateSearchTerms);
    }
}