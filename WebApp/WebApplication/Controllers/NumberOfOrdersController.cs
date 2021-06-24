using System.Web.Http;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/numberOfOrders")]
    public class NumberOfOrdersController : ApiController
    {
        private readonly IScalarFunctionsRepository _scalarFunctionsRepository;

        public NumberOfOrdersController()
        {

        }

        public NumberOfOrdersController(IScalarFunctionsRepository scalarFunctionsRepository)
        {
            _scalarFunctionsRepository = scalarFunctionsRepository;
        }

        [HttpGet]
       // [Authorize]
        [Route("getOrdersNumberForToday")]
        public int GetOrdersNumberForToday()
        {
            return _scalarFunctionsRepository.GetNumberOfOrdersForToday();
        }

       [HttpGet]
       // [Authorize]
       [Route("getOrdersNumberAllTime")]
       public int GetOrdersNumberAllTime()
       {
           return _scalarFunctionsRepository.GetNumberOfOrdersAllTime();
       }
    }
}