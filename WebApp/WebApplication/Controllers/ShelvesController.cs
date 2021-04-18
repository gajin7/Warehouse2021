using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/shelves")]
    public class ShelvesController : ApiController
    {
        private readonly IShelfRepository _shelfRepository;

        public ShelvesController()
        {

        }
        public ShelvesController(IShelfRepository shelfRepository)
        {
            _shelfRepository = shelfRepository;
        }

        [Route("getShelvesInWarehouse")]
        [HttpGet]
        public IEnumerable<ShelfItemsResult> GetShelvesInWarehouse([FromUri]string warehouseId)
        {
            var shelves = _shelfRepository.GetShelvesInWarehouse(warehouseId).ToList();
            return (from shelf in shelves let items = shelf.Items.Select(item => new ItemResult()
                { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Type = item.Type}).ToList()
                select new ShelfItemsResult()
                    { Name = shelf.Id, Items = items}).ToList();
        }

       
    }
}