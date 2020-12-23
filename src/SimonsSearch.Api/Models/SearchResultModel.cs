using SimonsSearch.Common.Enums;

namespace SimonsSearch.Api.Models
{
    public class SearchResultModel
    {
        public string Name { get; set; }

        public string Metadata { get; set; }

        public ResultType Type { get; set; }
    }
}