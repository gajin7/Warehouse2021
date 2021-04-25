using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public Vehicle GetVehicleForDriver(string driverId)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Vehicles.FirstOrDefault(i => i.DriverId.Equals(driverId));
            }
        }

        public IEnumerable<Vehicle> GetAllFreeVehicles()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Vehicles.Where(v => v.DriverId == null || v.DriverId == "").ToList();
            }
        }

        public OperationResult FreeVehicle(string registration)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {


                    var vehicle = accessDb.Vehicles.FirstOrDefault(v => v.Registration.Equals(registration));
                    if (vehicle != null)
                    {
                        vehicle.DriverId = null;
                    }

                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle successfully freed",
                            Success = true
                        };
                    }

                    return new OperationResult()
                    {
                        Message = "Vehicle can't be free at the moment. Please try again later",
                        Success = false
                    };
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Vehicle can't be free at the moment. Please try again later",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult TakeVehicleByDriver(string registration, string driverId)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var vehicle = accessDb.Vehicles.FirstOrDefault(v => v.Registration.Equals(registration));
                    if (vehicle != null)
                    {
                        vehicle.DriverId = driverId;
                    }

                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle successfully taken",
                            Success = true
                        };
                    }

                    return new OperationResult()
                    {
                        Message = "Vehicle can't be taken at the moment. Please try again later",
                        Success = false
                    };
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Vehicle can't be taken at the moment. Please try again later",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }

        }
    }
}