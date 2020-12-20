using System;
using SimonsSearch.Common.Enums;

namespace SimonsSearch.Core.Models
{
    public class LockDto : BaseDto
    {
        public Guid BuildingId { get; set; }

        public LockType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public string Floor { get; set; }

        public string RoomNumber { get; set; }
    }
}