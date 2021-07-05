using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Web.Model.Request
{
    public class WarehouseParameters
    {
        public string WarehouseId { get; set; }
        public string Address { get; set; }
        public string StorekeeperName { get; set; }
        public string StorekeeperEmail { get; set; }
        public string StorekeeperId { get; set; }
    }
}