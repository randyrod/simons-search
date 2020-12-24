using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimonsSearch.Api.Models;
using SimonsSearch.Core.Services;

namespace SimonsSearch.Api.Controllers
{
    [ApiController]
    [Route("/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public IActionResult Search([FromQuery] string query, [FromServices] ISearchService searchService)
        {
            var maybeResult = searchService.Search(query);

            if (!maybeResult.Any())
            {
                return NoContent();
            }

            var results = maybeResult.Select(x => new SearchResultModel
            {
                Id = x.Id,
                Name = x.Name,
                Metadata = x.Metadata,
                Type = x.Type.ToString("G")
            }).ToList();
            return Ok(results);
        }
    }
}