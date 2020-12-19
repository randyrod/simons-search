using SimonsSearch.Common.Enums;

namespace SimonsSearch.Core.Models
{
    public class SearchResultDto
    {
        public string Name { get; set; }

        public string Metadata { get; set; }

        public ResultType Type { get; set; }
    }
}