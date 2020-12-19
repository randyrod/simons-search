using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Data
{
    public class SimonsSearchDbContext
    {
        //TODO: use proper DB instead
        public List<Building> Buildings { get; set; }

        public List<Lock> Locks { get; set; }

        public List<Group> Groups { get; set; }

        public List<Medium> Mediums { get; set; }
    }
}