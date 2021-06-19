using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
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