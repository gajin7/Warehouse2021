using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IItemRepository
    {
        OperationResult AddItem(ItemRequest item);
        Item GetItem(string id);
        OperationResult ChangeQuantity(string id, int addToQuantity);
        IEnumerable<ItemResult> GetAllItems();
        int GetQuantityForItem(string id);
        OperationResult ChangeItem(ItemRequest item);
        OperationResult RemoveItem(string id);

    }
}