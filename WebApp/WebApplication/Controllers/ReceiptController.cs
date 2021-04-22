using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Controllers.Parameters;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/receipt")]
    public class ReceiptController : ApiController
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IItemRepository _itemRepository;

        public ReceiptController()
        {
        }

        public ReceiptController(IReceiptRepository receiptRepository,IItemRepository itemRepository)
        {
            _receiptRepository = receiptRepository;
            _itemRepository = itemRepository;
        }

        [HttpPost]
        [Authorize]
        [Route("createReceipt")]
        public OperationResult CreateReceipt(ReceiptParameters parameters)
        {
            var itemList = new List<Item>();
            foreach (var item in parameters.Items)
            {
                var foundItem = _itemRepository.GetItem(item.Id);
                foundItem.Quantity = item.Quantity;
                itemList.Add(foundItem);
            }
            return _receiptRepository.CreateReceipt(itemList, parameters.Company);
        }
    }
}