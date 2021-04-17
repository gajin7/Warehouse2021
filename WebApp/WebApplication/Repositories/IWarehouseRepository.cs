using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Repositories
{
    public interface IWarehouseRepository
    {
        Warehouse GetWarehouse(string id);
        IEnumerable<Warehouse> GetAllWarehouses();
    }
}