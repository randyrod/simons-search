using SimonsSearch.Core.Interfaces;

namespace SimonsSearch.Core.Models
{
    public class BuildingDto : BaseDto, IHasTransitoryWeight
    {
        public string ShortCut { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TransitoryWeight { get; set; }

        public override string ToString()
        {
            var result = string.Empty;

            if (!string.IsNullOrWhiteSpace(Description))
            {
                result += $"Description: {Description}, ";
            }

            if (!string.IsNullOrWhiteSpace(ShortCut))
            {
                result += $"Shortcut: {ShortCut}";
            }

            return result;
        }
    }
}