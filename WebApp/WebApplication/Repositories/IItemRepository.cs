using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IItemRepository
    {
        OperationResult AddItem(Item item);
        Item GetItem(string id);
        OperationResult ChangeQuantity(string id, double addToQuantity);
        IEnumerable<Item> GetAllItems();
        IEnumerable<Item> GetItemsOnShelf(string shelfId);

        int GetQuantityForItem(string id);

    }
}