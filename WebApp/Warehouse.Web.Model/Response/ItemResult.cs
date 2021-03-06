
namespace Warehouse.Web.Model.Response
{
    public class ItemResult
    {
        public ItemResult()
        {

        }
        public string Id {get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public string Type { get; set; }
        public double? Amount { get; set; }
        public string Warehouse { get; set; }
        public string ShelfId { get; set; }
    }
}