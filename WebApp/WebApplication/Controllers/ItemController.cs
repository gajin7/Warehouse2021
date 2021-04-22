using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        private readonly IItemRepository _itemRepository;
        private readonly ISearchService _searchService;
        public ItemController(IItemRepository itemRepository, ISearchService searchService)
        {
            _itemRepository = itemRepository;
            _searchService = searchService;
        }

        [HttpPost]
        [Route("addItem")]
        public OperationResult AddItem([FromBody]Item item)
        {
            return _itemRepository.AddItem(item);
        }

        [HttpGet]
        [Route("getItem")]
        public Item GetItem(string id)
        {
            return _itemRepository.GetItem(id);
        }

        [HttpGet]
        [Authorize]
        [Route("getAllItems")]
        public IEnumerable<ItemResult> GetAllItems()
        {
            var items1 = _itemRepository.GetAllItems();
            var items2 = _itemRepository.GetAllItems();
            var items3 = _itemRepository.GetAllItems();
            var items4 = _itemRepository.GetAllItems();
            var items5 = _itemRepository.GetAllItems();
            var items6 = _itemRepository.GetAllItems();
            var items7 = _itemRepository.GetAllItems();

            var allItems = items1.Concat(items2)
                .Concat(items2).Concat(items3).Concat(items4).Concat(items5).Concat(items6).Concat(items7).ToList();
            return allItems
                .Select(item => new ItemResult()
                {
                    Amount = item.Amount,
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Type = item.Type,
                    Warehouse = item.Shelf.WarehouseId
                })
                .ToList();
        }

        [HttpPost]
        [Authorize]
        [Route("changeQuantity")]
        public OperationResult ChangeQuantity(string id, double quantityChanger, bool negative)
        {
            if (negative)
            {
                quantityChanger = System.Math.Abs(quantityChanger) * (-1);
            }
            return _itemRepository.ChangeQuantity(id, quantityChanger);
        }

        [HttpGet]
        [Route("getQuantity")]
        public QuantityResult GetQuantity(string id)
        {
            return new QuantityResult()
            {
                quantity = _itemRepository.GetQuantityForItem(id)
            };
        }

        [Route("getAllItemsSearch")]
        [Authorize]
        [HttpGet]
        public IEnumerable<ItemResult> GetAllItemsSearch([FromUri]string warehouseId, string keyWord)
        {
            var items = _itemRepository.GetAllItems()
                .Select(item => new ItemResult()
                {
                    Amount = item.Amount,
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Type = item.Type,
                    Warehouse = item.Shelf.WarehouseId
                })
                .ToList();

            return _searchService.FilterAllItemsBaseOnKeyWord(items, keyWord).ToList();
        }
    }
}