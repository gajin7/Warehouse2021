
namespace WebApplication.Models
{
    public class LoadResult
    {
        public string Id { get; set; }
        public string Warehouse { get; set; }
        public string Ramp { get; set; }
        public bool RampFree { get; set; }
        public string Driver { get; set; }
        public string Vehicle { get; set; }
        public string Storekeeper { get; set; }
        public string ReportId { get; set; }
        public string ReceiptId { get; set; }
    }
}