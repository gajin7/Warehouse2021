using System.Collections.Generic;

namespace WebApplication.Repositories
{
    public interface IShelfRepository
    {
        IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId);
        IEnumerable<Shelf> GetShelves(string warehouseId);
    }
}
