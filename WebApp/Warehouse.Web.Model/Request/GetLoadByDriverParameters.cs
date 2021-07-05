using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Web.Model.Request
{
    public class GetLoadByDriverParameters
    {
        public string Email { get; set; }
        public string LoadId { get; set; }
    }
}