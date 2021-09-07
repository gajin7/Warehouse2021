using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Web.Model.Request
{
    public class ItemRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public string Type { get; set; }
        public string ShelfId { get; set; }
        public double? Amount { get; set; }
    }
}
