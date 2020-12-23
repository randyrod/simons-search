using System.Collections.Generic;
using SimonsSearch.Data.Entities;

namespace SimonsSearch.Data
{
    public class SimonsSearchDataContext : ISimonsSearchDataContext
    {
        public List<Building> Buildings { get; set; }

        public List<Lock> Locks { get; set; }

        public List<Group> Groups { get; set; }

        public List<Medium> Mediums { get; set; }

        public void Init(SimonsSearchDataSeederDto data)
        {
            if (data == null)
            {
                return;
            }

            Buildings = data.Buildings;
            Locks = data.Locks;
            Groups = data.Groups;
            Mediums = data.Media;
        }
    }
}