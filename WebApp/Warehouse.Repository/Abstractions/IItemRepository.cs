using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IItemRepository
    {
        OperationResult AddItem(Item item);
        Item GetItem(string id);
        OperationResult ChangeQuantity(string id, int addToQuantity);
        IEnumerable<ItemResult> GetAllItems();
        int GetQuantityForItem(string id);
        OperationResult ChangeItem(Item item);
        OperationResult RemoveItem(string id);

    }
}