using System.Collections.Generic;
using SimonsSearch.Core.Models;

namespace SimonsSearch.Core.Interfaces
{
    public interface IGroupWeightCalculator
    {
        void CalculateWeights(IReadOnlyList<GroupDto> groups, string originalSearchQuery, IReadOnlyList<string> separateSearchTerms);
    }
}