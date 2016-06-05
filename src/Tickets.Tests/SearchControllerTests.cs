using GogoKit;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Controllers;
using Tickets.Models;

namespace Tickets.Tests
{
    public class SearchControllerTests
    {
        [TestClass]
        public class GetAsync
        {
            [TestMethod]
            public async Task ShouldReturnCorrectlyFormattedResults()
            {
                // Arrange
                var viagogoService = new Mock<IViagogoClient>();
                var serviceResponse = new PagedResource<SearchResult>
                {
                    TotalItems = 20,
                    Items = new List<SearchResult>
                    {
                        new SearchResult
                        {
                            CategoryLink = new Link
                            {
                                HRef = "https://api.viagogo.net/v2/categories/20746"
                            },
                            Title = "Beyonce"
                        }
                    }
                };

                viagogoService
                    .Setup(client => client.Search.GetAsync("Bey", It.IsAny<SearchResultRequest>()))
                    .ReturnsAsync(serviceResponse);

                var expected = serviceResponse
                    .Items
                    .Select(TicketsSuggestion.FromSearchResult)
                    .ToArray();

                var controller = new SearchController(viagogoService.Object);

                // Act
                var result = await controller.GetAsync("Bey");

                // Assert
                Assert.IsInstanceOfType(result, typeof(JsonResult));

                var actual = (TicketsSuggestion[])((JsonResult)result).Value;

                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
