using System.Linq;


namespace WebApplication.Repositories
{
    public class RampRepository : IRampRepository
    {
        public Ramp GetRampForLoad(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var ramp =  accessDb.Ramps.FirstOrDefault(r => r.WarehouseId.Equals(warehouseId) && r.Free) ?? accessDb.Ramps.FirstOrDefault(r => r.WarehouseId.Equals(warehouseId));
                if (ramp != null) ramp.Free = false;
                accessDb.SaveChanges();
                return ramp;
            }
        }
    }
}