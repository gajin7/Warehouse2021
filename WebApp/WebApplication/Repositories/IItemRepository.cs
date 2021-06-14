using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
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