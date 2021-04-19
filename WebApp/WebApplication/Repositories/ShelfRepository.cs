using System.Collections.Generic;
using System.Linq;

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
            var shelves = _accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId));
            foreach (var shelf in shelves)
            {
                foreach (var item in shelf.Items)
                {
                    if (item.Quantity != 0) continue;
                    shelf.Items.Remove(item);
                    break;
                }
            }

            return shelves;

        }
    }
}