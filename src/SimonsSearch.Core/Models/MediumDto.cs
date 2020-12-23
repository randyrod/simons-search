using System;
using SimonsSearch.Common.Enums;

namespace SimonsSearch.Core.Models
{
    public class MediumDto : BaseDto
    {
        public Guid GroupId { get; set; }

        public MediumType Type { get; set; }

        public string Owner { get; set; }

        public string SerialNumber { get; set; }

        public string Description { get; set; }
    }
}