using System;
using SimonsSearch.Common.Enums;

namespace SimonsSearch.Data.Entities
{
    public class Medium : BaseEntity
    {
        public Guid GroupId { get; set; }

        public MediumType Type { get; set; }

        public string Owner { get; set; }

        public string SerialNumber { get; set; }

        public string Description { get; set; }
    }
}