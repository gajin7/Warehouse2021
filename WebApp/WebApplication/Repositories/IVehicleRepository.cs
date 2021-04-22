using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Repositories
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicleForDriver(string driverId);
    }
}