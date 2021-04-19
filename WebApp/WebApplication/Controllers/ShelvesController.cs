using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/shelves")]
    public class ShelvesController : ApiController
    {
        private readonly IShelfRepository _shelfRepository;
        private readonly ISearchService _searchService;

        public ShelvesController()
        {

        }
        public ShelvesController(IShelfRepository shelfRepository, ISearchService searchService)
        {
            _shelfRepository = shelfRepository;
            _searchService = searchService;
        }

        [Route("getShelvesInWarehouse")]
        [Authorize]
        [HttpGet]
        public IEnumerable<ShelfItemsResult> GetShelvesInWarehouse([FromUri]string warehouseId)
        {
            var shelves = _shelfRepository.GetShelvesInWarehouse(warehouseId).ToList();
            return (from shelf in shelves let items = shelf.Items.Select(item => new ItemResult()
                { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Type = item.Type, Amount = item.Amount}).ToList()
                select new ShelfItemsResult()
                    { Name = shelf.Id, Items = items}).ToList();
        }

        [Route("getShelvesInWarehouseSearch")]
        [Authorize]
        [HttpGet]
        public IEnumerable<ShelfItemsResult> GetShelvesInWarehouseSearch([FromUri]string warehouseId, string keyWord)
        {
            var shelves = _shelfRepository.GetShelvesInWarehouse(warehouseId).ToList();
           
            
            var shelfItems = (from shelf in shelves
                         let items = shelf.Items.Select(item => new ItemResult()
                    { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Type = item.Type, Amount = item.Amount }).ToList()
                select new ShelfItemsResult()
                    { Name = shelf.Id, Items = items }).ToList();

            return  _searchService.FilterShelvesItemsBaseOnKeyWord(shelfItems, keyWord).ToList();
        }


    }
}