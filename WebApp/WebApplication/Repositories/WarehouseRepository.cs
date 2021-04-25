using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApplication.Repositories
{
    [RoutePrefix("api/warehouse")]
    public class WarehouseRepository : IWarehouseRepository
    {
        [Route("getAllWarehouses")]
        public IEnumerable<Warehouse> GetAllWarehouses()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Warehouses.ToList();
            }
        }

        
        public Warehouse GetWarehouse(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Warehouses.FirstOrDefault(i => i.Id.Equals(id));
            }
        }
    }
}