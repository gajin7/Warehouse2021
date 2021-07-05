using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IShelfRepository
    {
        IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId);
        IEnumerable<Shelf> GetShelves(string warehouseId);
        OperationResult AddShelf(string warehouseId, string shelfId);
        OperationResult RemoveShelf(string warehouseId, string shelfId);
    }
}
