using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;
using WebApplication.Repositories;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypesRepository _itemTypesRepository;
        private readonly ICompaniesRepository _companiesRepository;
        private readonly ISearchService _searchService;
        private readonly IReportRepository _reportRepository;
        public ItemController(IItemRepository itemRepository, ISearchService searchService,
            IItemTypesRepository itemTypesRepository, ICompaniesRepository companiesRepository,
            IReportRepository reportRepository)
        {
            _itemRepository = itemRepository;
            _searchService = searchService;
            _itemTypesRepository = itemTypesRepository;
            _companiesRepository = companiesRepository;
            _reportRepository = reportRepository;
        }

        [HttpPost]
        [Route("addItem")]
        public OperationResult AddItem([FromBody]Item item)
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
            if (itemResult.Success)
            {
                var reportResult = _reportRepository.CreateInReport(item);
                if (reportResult.Success)
                {
                    return itemResult;
                }
                else
                {
                    return reportResult;
                }
            }
            else
            {
                return itemResult;
            }
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
            return _itemRepository.GetAllItems().ToList();
        }

        [HttpPost]
        [Authorize]
        [Route("changeQuantity")]
        public OperationResult ChangeQuantity(string id, int quantityChanger)
        {
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
            var items = _itemRepository.GetAllItems();
            return _searchService.FilterAllItemsBaseOnKeyWord(items, keyWord).ToList();
        }

        [HttpGet]
        [Route("getItemTypes")]
        [Authorize]
        public IEnumerable<string> GetItemTypes()
        {
            return _itemTypesRepository.GetItemTypes();
        }

        [HttpGet]
        [Route("getCompanies")]
        public IEnumerable<CompanyResult> GetCompanies()
        {
            return _companiesRepository.GetCompanies().ToList().Select(c => new CompanyResult()
            {
                AccountNo = c.AccountNo,
                Address = c.Address,
                Name = c.Name,
                PIB = c.PIB
            });
        }
    }
}