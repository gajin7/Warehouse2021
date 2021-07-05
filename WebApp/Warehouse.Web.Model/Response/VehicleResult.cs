
namespace Warehouse.Web.Model.Response
{
    public class VehicleResult
    {
        public string Registration { get; set; }
        public string Type { get; set; }
        public int? LoadCapacity { get; set; }
        public string Mileage { get; set; }
        public int? ProductionYear { get; set; }
        public string Brand { get; set; }
        public bool Free { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
    }
}