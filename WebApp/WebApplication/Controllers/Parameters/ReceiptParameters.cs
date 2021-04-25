using System.Collections.Generic;
using WebApplication.Models;


namespace WebApplication.Controllers.Parameters
{
    public class ReceiptParameters
    {
        public IEnumerable<ItemResult> Items { get; set; }
        public string Company { get; set; }
        public string StorekeeperEmail { get; set; }
        public string WarehouseId { get; set; }
    }
}