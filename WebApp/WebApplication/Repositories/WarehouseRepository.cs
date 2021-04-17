using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {

        private readonly AccessDb _accessDb;
        public WarehouseRepository(AccessDb accessDb)
        {
            _accessDb = accessDb;
        }
        public IEnumerable<Warehouse> GetAllWarehouses()
        {
            return _accessDb.Warehouses.ToList();
        }

        public Warehouse GetWarehouse(string id)
        {
            return _accessDb.Warehouses.FirstOrDefault(i => i.Id.Equals(id));
        }
    }
}