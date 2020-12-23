using System;
using SimonsSearch.Common.Enums;

namespace SimonsSearch.Core.Models
{
    public class SearchResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Metadata { get; set; }

        public ResultType Type { get; set; }

        public int Weight { get; set; }
    }
}