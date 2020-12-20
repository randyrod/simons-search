using SimonsSearch.Core.Interfaces;

namespace SimonsSearch.Core.Models
{
    public class BuildingDto : BaseDto, IHasTransitoryWeight
    {
        public string ShortCut { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TransitoryWeight { get; set; }
    }
}