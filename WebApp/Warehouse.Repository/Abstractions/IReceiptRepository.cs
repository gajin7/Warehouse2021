using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IReceiptRepository
    {
        ReceiptOperationResult CreateReceipt(IEnumerable<ItemResult> items, string company);
        IEnumerable<Receipt> GetReceipts();
        Receipt GetReceipt(string id);
        IEnumerable<ReceiptItem> GetReceiptItems(string id);

    }
}
