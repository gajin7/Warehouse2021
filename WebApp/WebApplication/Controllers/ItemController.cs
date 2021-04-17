using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        private readonly IItemRepository _itemRepository;
        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
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
        [Route("getAllItems")]
        public IEnumerable<Item> GetAllItems()
        {
            return _itemRepository.GetAllItems();
        }

        [HttpPost]
        [Route("changeQuantity")]
        public OperationResult ChangeQuantity(string id, double quantityChanger)
        {
            return _itemRepository.ChangeQuantity(id, quantityChanger);
        }
    }
}