using GogoKit.Models.Response;
using System.Collections.Generic;

namespace Tickets.Models
{
    public class TicketsSuggestion : EqualityComparer<TicketsSuggestion>
    {
        public string CategoryId { get; set; }
        public string Title { get; set; }

        public static TicketsSuggestion FromSearchResult(SearchResult sr)
        {
            string categoryHRef = sr.CategoryLink.HRef;
            return new TicketsSuggestion
            {
                CategoryId = categoryHRef.Substring(categoryHRef.LastIndexOf('/') + 1),
                Title = sr.Title
            };
        }

        #region EqualityComparer

        public override bool Equals(object obj)
        {
            return obj != null &&
                   obj is TicketsSuggestion &&
                   Equals(this, (TicketsSuggestion)obj);
        }

        public override bool Equals(TicketsSuggestion x, TicketsSuggestion y)
        {
            return x.CategoryId.Equals(y.CategoryId) &&
                   x.Title.Equals(y.Title);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override int GetHashCode(TicketsSuggestion obj)
        {
            return $"{CategoryId}-{Title}".GetHashCode();
        }

        #endregion
    }
}
