using System.Collections.Generic;
using Warehouse.Web.Model.Response;

namespace Warehouse.Web.Model.Request
{
    public class ReceiptParameters
    {
        public IEnumerable<ItemResult> Items { get; set; }
        public string Company { get; set; }
        public string StorekeeperEmail { get; set; }
        public string WarehouseId { get; set; }
    }
}