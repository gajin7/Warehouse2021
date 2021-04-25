using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicleForDriver(string driverId);
        IEnumerable<Vehicle> GetAllFreeVehicles();
        OperationResult FreeVehicle(string registration);
        OperationResult TakeVehicleByDriver(string registration, string driverId);
    }
}