using System.Collections.Generic;
using SimonsSearch.Common.Enums;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public interface IMediumRepository
    {
        IReadOnlyList<Medium> GetMediumsMatchingTerms(IReadOnlyList<string> searchTerms, IReadOnlyList<MediumType> mediumTypes);
    }
}