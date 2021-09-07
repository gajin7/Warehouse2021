using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;


namespace WebApplication.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypesRepository _itemTypesRepository;
        private readonly ISearchService _searchService;
        private readonly IReportRepository _reportRepository;
        public ItemController(IItemRepository itemRepository, ISearchService searchService,
            IItemTypesRepository itemTypesRepository, IReportRepository reportRepository)
        {
            _itemRepository = itemRepository;
            _searchService = searchService;
            _itemTypesRepository = itemTypesRepository;
            _reportRepository = reportRepository;
        }

        [HttpPost]
        [Route("addItem")]
        [Authorize(Roles = "admin,storekeeper")]
        public OperationResult AddItem([FromBody]ItemRequest item)
        {
            if (item == null || item.Name.IsNullOrWhiteSpace() || !item.Amount.HasValue || item.ShelfId.IsNullOrWhiteSpace() || !item.Quantity.HasValue)
            {
                return new OperationResult()
                {
                    Message = "Validation failed, please check your dana and try again",
                    Success =  false
                };
            }
            var itemResult = _itemRepository.AddItem(item);
            if (!itemResult.Success)
            {
                return itemResult;
            }
            var reportResult = _reportRepository.CreateInReport(item);

            return reportResult.Success ? itemResult : reportResult;

        }

        [HttpGet]
        [Authorize(Roles = "admin,storekeeper")]
        [Route("getItem")]
        public Item GetItem(string id)
        {
            return _itemRepository.GetItem(id);
        }

        [HttpGet]
        [Authorize(Roles = "admin,storekeeper")]
        [Route("getAllItems")]
        public IEnumerable<ItemResult> GetAllItems()
        {
            return _itemRepository.GetAllItems().ToList();
        }

        [HttpPost]
        [Authorize(Roles = "admin,storekeeper")]
        [Route("changeQuantity")]
        public OperationResult ChangeQuantity(string id, int quantityChanger)
        {
            return _itemRepository.ChangeQuantity(id, quantityChanger);
        }

        [HttpGet]
        [Authorize(Roles = "admin,storekeeper")]
        [Route("getQuantity")]
        public QuantityResult GetQuantity(string id)
        {
            return new QuantityResult()
            {
                quantity = _itemRepository.GetQuantityForItem(id)
            };
        }

        [Route("getAllItemsSearch")]
        [Authorize(Roles = "admin,storekeeper")]
        [HttpGet]
        public IEnumerable<ItemResult> GetAllItemsSearch([FromUri]string warehouseId, string keyWord)
        {
            var items = _itemRepository.GetAllItems();
            return _searchService.FilterAllItemsBaseOnKeyWord(items, keyWord).ToList();
        }

        [HttpGet]
        [Route("getItemTypes")]
        [Authorize(Roles = "admin,storekeeper")]
        public IEnumerable<string> GetItemTypes()
        {
            return _itemTypesRepository.GetItemTypes();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("changeItem")]
        public OperationResult ChangeItem([FromBody]ItemRequest item)
        {
            return _itemRepository.ChangeItem(item);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("removeItem")]
        public OperationResult RemoveItem([FromUri] string id)
        {
            return _itemRepository.RemoveItem(id);
        }
    }
}