using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        public IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var shelves = accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId)).ToList();
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

        public IEnumerable<Shelf> GetShelves(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var shelves = accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId)).ToList();

                return shelves;
            }
        }
    }
}