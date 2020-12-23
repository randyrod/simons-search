using SimonsSearch.Core.Interfaces;

namespace SimonsSearch.Core.Models
{
    public class GroupDto : BaseDto, IHasTransitoryWeight
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int TransitoryWeight { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description) ? string.Empty : $"Description: {Description}";
        }
    }
}