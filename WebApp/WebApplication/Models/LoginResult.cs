using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LoginResult : OperationResult
    {
        public string Role { get; set; }
    }
}