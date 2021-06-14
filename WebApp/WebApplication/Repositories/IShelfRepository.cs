using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IShelfRepository
    {
        IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId);
        IEnumerable<Shelf> GetShelves(string warehouseId);
        OperationResult AddShelf(string warehouseId, string shelfId);
        OperationResult RemoveShelf(string warehouseId, string shelfId);
    }
}
