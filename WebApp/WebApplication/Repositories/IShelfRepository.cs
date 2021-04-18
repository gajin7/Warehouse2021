
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IShelfRepository
    {
        IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId);
    }
}
