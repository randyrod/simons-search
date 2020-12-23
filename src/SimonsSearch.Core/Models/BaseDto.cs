using System;

namespace SimonsSearch.Core.Models
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }

        public int Weight { get; set; }
    }
}