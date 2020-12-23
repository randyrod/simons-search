using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Data
{
    public interface ISimonsSearchDataContext
    {
        List<Building> Buildings { get; set; }

        List<Lock> Locks { get; set; }

        List<Group> Groups { get; set; }

        List<Medium> Mediums { get; set; }

        void Init(SimonsSearchDataSeederDto data);
    }
}