using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Data
{
    public class SimonsSearchDataSeederDto
    {
        public List<Building> Buildings { get; set; }

        public List<Lock> Locks { get; set; }

        public List<Group> Groups { get; set; }

        public List<Medium> Media { get; set; }
    }
}