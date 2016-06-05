using GogoKit;
using GogoKit.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Controllers
{
    public class TicketController : BaseServiceController
    {
        public TicketController(IViagogoClient viagogoClient)
            : base(viagogoClient) { }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int eventId, int page, int? numberOfTickets)
        {
            var listingRequest = new ListingRequest
            {
                Page = page,
                PageSize = PAGE_SIZE,
                NumberOfTickets = numberOfTickets
            };

            var results = await ViagogoClient.Listings.GetByEventAsync(eventId, listingRequest);

            var items = results.Items.Select(TicketsListing.FromListing);

            return new OkObjectResult(new
            {
                Page = page,
                TotalItems = results.TotalItems,
                Items = items.ToArray()
            });
        }
    }
}
