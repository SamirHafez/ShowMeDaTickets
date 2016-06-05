using GogoKit;
using GogoKit.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Controllers
{
    public class EventController : BaseServiceController
    {
        public EventController(IViagogoClient viagogoClient)
            : base(viagogoClient) { }

        [HttpGet]
        public async Task<IActionResult> GetAsync(
            int categoryId,
            int page,
            DateTimeOffset? minStartDate,
            DateTimeOffset? maxStartDate)
        {
            var eventRequest = new EventRequest
            {
                Page = page,
                PageSize = PAGE_SIZE,
                MinStartDate = minStartDate,
                MaxStartDate = maxStartDate,
                Sort = new List<Sort<EventSort>>
                {
                    new Sort<EventSort>(EventSort.MinTicketPrice, SortDirection.Ascending)
                },
                OnlyWithTickets = true
            };

            var results = await ViagogoClient.Events.GetByCategoryAsync(categoryId, eventRequest);

            var items = results.Items.Select(TicketsEvent.FromEvent);

            return new OkObjectResult(new
            {
                Page = page,
                TotalItems = results.TotalItems,
                Items = SetLowestPrices(items.ToArray())
            });
        }

        private TicketsEvent[] SetLowestPrices(TicketsEvent[] items)
        {
            var countryGroups = items
                .OrderBy(item => item.MinTicketPrice.Amount)
                .GroupBy(item => item.Venue.Country)
                .Where(group => group.Count() > 1);

            foreach (var group in countryGroups)
                group.First().LowestPrice = true;

            return items;
        }
    }
}
