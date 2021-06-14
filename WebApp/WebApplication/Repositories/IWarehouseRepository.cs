using System.Collections.Generic;
using WebApplication.Controllers.Parameters;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IWarehouseRepository
    {
        Warehouse GetWarehouse(string id);
        IEnumerable<Warehouse> GetAllWarehouses();
        OperationResult AddWarehouse(WarehouseParameters warehouse);
        OperationResult ChangeWarehouse(WarehouseParameters warehouse);
    }
}