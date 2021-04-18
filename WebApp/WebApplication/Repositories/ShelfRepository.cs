using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        private readonly AccessDb _accessDb;
        private readonly IItemRepository _itemRepository;
        public ShelfRepository(AccessDb accessDb, IItemRepository itemRepository)
        {
            _accessDb = accessDb;
            _itemRepository = itemRepository;
        }
        public IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId)
        {
            return _accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId));
            
        }
    }
}