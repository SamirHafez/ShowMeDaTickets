using GogoKit.Models.Response;

namespace Tickets.Models
{
    public class TicketsListing
    {
        public int? Id { get; set; }
        public int? NumberOfTickets { get; set; }
        public string EstimatedTotalTicketPrice { get; set; }
        public string EstimatedBookingFee { get; set; }
        public string EstimatedTotalCharge { get; set; }
        public string Section { get; set; }

        public static TicketsListing FromListing(Listing listing)
        {
            return new TicketsListing
            {
                Id = listing.Id,
                NumberOfTickets = listing.NumberOfTickets,
                EstimatedTotalTicketPrice = listing.EstimatedTotalTicketPrice.Display,
                EstimatedBookingFee = listing.EstimatedBookingFee.Display,
                EstimatedTotalCharge = listing.EstimatedTotalCharge.Display,
                Section = listing.Seating.Section
            };
        }
    }
}