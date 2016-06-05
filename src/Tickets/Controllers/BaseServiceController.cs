using GogoKit;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.Controllers
{
    [Route("api/[Controller]")]
    public abstract class BaseServiceController : Controller
    {
        protected const int PAGE_SIZE = 10;

        protected readonly IViagogoClient ViagogoClient;

        public BaseServiceController(IViagogoClient viagogoClient)
        {
            ViagogoClient = viagogoClient;
        }
    }
}
