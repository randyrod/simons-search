using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Core.Repositories
{
    public interface IMediumRepository
    {
        IReadOnlyList<Medium> GetMediumsMatchingTerms(IReadOnlyList<string> searchTerms);
    }
}