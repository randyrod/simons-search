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

        public override string ToString()
        {
            var result = string.Empty;

            if (!string.IsNullOrWhiteSpace(Description))
            {
                result += $"Description: {Description}, ";
            }

            result += $"Type: {Type:G}, ";

            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                result += $"Serial number: {SerialNumber}, ";
            }

            if (!string.IsNullOrWhiteSpace(Floor))
            {
                result += $"Floor: {Floor}, ";
            }

            if (!string.IsNullOrWhiteSpace(RoomNumber))
            {
                result += $"Room number: {RoomNumber}";
            }

            return result;
        }
    }
}