using GogoKit;
using GogoKit.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Controllers
{
    public class SearchController : BaseServiceController
    {
        public SearchController(IViagogoClient viagogoClient)
            : base(viagogoClient) { }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string query)
        {
            var searchRequest = new SearchResultRequest
            {
                Type = new List<SearchResultTypeFilter>
                {
                    SearchResultTypeFilter.Category
                },
                PageSize = PAGE_SIZE
            };

            var results = await ViagogoClient.Search.GetAsync(query, searchRequest);

            var items = results.Items.Select(TicketsSuggestion.FromSearchResult);

            return Json(items.ToArray());
        }
    }
}
