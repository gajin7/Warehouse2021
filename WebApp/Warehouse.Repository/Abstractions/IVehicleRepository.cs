using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicleForDriver(string driverId);
        IEnumerable<VehicleResult> GetAllVehicles();
        OperationResult AddVehicle(VehicleResult vehicle);
        OperationResult ChangeVehicle(VehicleResult vehicle);
        OperationResult RemoveVehicle(string registration);
        IEnumerable<Vehicle> GetAllFreeVehicles();
        OperationResult FreeVehicle(string registration);
        OperationResult TakeVehicleByDriver(string registration, string driverId);
    }
}