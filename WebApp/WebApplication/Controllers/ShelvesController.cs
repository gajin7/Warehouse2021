using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Response;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/shelves")]
    public class ShelvesController : ApiController
    {
        private readonly IShelfRepository _shelfRepository;
        private readonly ISearchService _searchService;
        private readonly IPricelistRepository _pricelistRepository;

        public ShelvesController()
        {

        }
        public ShelvesController(IShelfRepository shelfRepository, ISearchService searchService, IPricelistRepository pricelistRepository)
        {
            _shelfRepository = shelfRepository;
            _searchService = searchService;
            _pricelistRepository = pricelistRepository;
        }

        [Route("getShelvesInWarehouse")]
        [Authorize]
        [HttpGet]
        public IEnumerable<ShelfItemsResult> GetShelvesInWarehouse([FromUri]string warehouseId)
        {
            var shelves = _shelfRepository.GetShelvesInWarehouse(warehouseId).ToList();
            var retVal = (from shelf in shelves let items = shelf.Items.Select(item => new ItemResult()
                { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Type = item.Type, Amount = _pricelistRepository.GetPriceForItem(item.Id)}).ToList()
                select new ShelfItemsResult()
                    { Name = shelf.Id, Items = items}).ToList();

            return retVal;
        }

        [Route("getShelves")]
        [Authorize]
        [HttpGet]
        public IEnumerable<string> GetShelves([FromUri]string warehouseId)
        {
            return _shelfRepository.GetShelves(warehouseId).Select(s => s.Id);
        }

        [Route("getShelvesInWarehouseSearch")]
        [Authorize]
        [HttpGet]
        public IEnumerable<ShelfItemsResult> GetShelvesInWarehouseSearch([FromUri]string warehouseId, string keyWord)
        {
            var shelves = _shelfRepository.GetShelvesInWarehouse(warehouseId).ToList();
           
            
            var shelfItems = (from shelf in shelves
                         let items = shelf.Items.Select(item => new ItemResult()
                    { Id = item.Id, Name = item.Name, Quantity = item.Quantity, Type = item.Type, Amount = _pricelistRepository.GetPriceForItem(item.Id) }).ToList()
                select new ShelfItemsResult()
                    { Name = shelf.Id, Items = items }).ToList();

            return  _searchService.FilterShelvesItemsBaseOnKeyWord(shelfItems, keyWord).ToList();
        }

        [HttpGet]
        [Authorize]
        [Route("addShelf")]
        public OperationResult AddShelf([FromUri]string warehouseId, string shelfId)
        {
            return _shelfRepository.AddShelf(warehouseId, shelfId);
        }

        [HttpDelete]
        [Authorize]
        [Route("removeShelf")]
        public OperationResult RemoveShelf([FromUri]string warehouseId, string shelfId)
        {
            return _shelfRepository.RemoveShelf(warehouseId, shelfId);
        }

    }
}