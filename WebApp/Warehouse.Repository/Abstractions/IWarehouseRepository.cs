using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IWarehouseRepository
    {
        Model.Warehouse GetWarehouse(string id);
        IEnumerable<Model.Warehouse> GetAllWarehouses();
        OperationResult AddWarehouse(WarehouseParameters warehouse);
        OperationResult ChangeWarehouse(WarehouseParameters warehouse);
    }
}