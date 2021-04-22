using System.Linq;

namespace WebApplication.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AccessDb _accessDb;
        public VehicleRepository(AccessDb accessDb)
        {
            _accessDb = accessDb;
        }
        public Vehicle GetVehicleForDriver(string driverId)
        {
            return _accessDb.Vehicles.FirstOrDefault(i => i.DriverId.Equals(driverId));
        }
    }
}