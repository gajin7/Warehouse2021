using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Web.Model.Response
{
    public class LoginResult : OperationResult
    {
        public string Role { get; set; }
    }
}