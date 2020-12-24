using System;
using SimonsSearch.Common.Enums;

namespace SimonsSearch.Api.Models
{
    public class SearchResultModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Metadata { get; set; }

        public string Type { get; set; }
    }
}