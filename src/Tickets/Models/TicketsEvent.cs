using GogoKit.Models.Response;
using System;

namespace Tickets.Models
{
    public class TicketsEvent
    {
        public int? Id { get; set; }
        public Money MinTicketPrice { get; set; }
        public TicketsVenue Venue { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public bool LowestPrice { get; set; }

        public static TicketsEvent FromEvent(Event @event)
        {
            return new TicketsEvent
            {
                Id = @event.Id,
                MinTicketPrice = @event.MinTicketPrice,
                Venue = new TicketsVenue
                {
                    Name = @event.Venue.Name,
                    Country = @event.Venue.Country.Name
                },
                StartDate = @event.StartDate,
                LowestPrice = false
            };
        }
    } 
} 