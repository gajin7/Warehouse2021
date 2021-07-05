using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository
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

        public IEnumerable<VehicleResult> GetAllVehicles()
        {
            using (var accessDb = new AccessDb())
            {
                var vehicles = accessDb.Vehicles.ToList();
                return vehicles.Select(vehicle => new VehicleResult()
                    {
                        Registration = vehicle.Registration,
                        Brand = vehicle.Brand,
                        LoadCapacity = vehicle.LoadCapacity,
                        Mileage = vehicle.Mileage,
                        ProductionYear = vehicle.ProductionYear,
                        Type = vehicle.Type,
                        Free = string.IsNullOrEmpty(vehicle.DriverId),
                        DriverId = vehicle.DriverId,
                        DriverName = vehicle.Employee?.FirstName + " " + vehicle.Employee?.LastName
                    })
                    .ToList();
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

        public OperationResult AddVehicle(VehicleResult vehicle)
        {
            if (vehicle.Registration.IsNullOrWhiteSpace())
            {
                return new OperationResult()
                {
                    Success = false,
                    Message = "Please add vehicle registration"
                };
            }
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Vehicles.FirstOrDefault(v => v.Registration.Equals(vehicle.Registration)) != null)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle with same registration already exist.Vehicle can't be added.",
                            Success = false
                        };
                    }

                    accessDb.Vehicles.Add(new Vehicle
                    {
                        Registration = vehicle.Registration,
                        Brand = vehicle.Brand,
                        LoadCapacity = vehicle.LoadCapacity,
                        ProductionYear = vehicle.ProductionYear,
                        Mileage = vehicle.Mileage
                    });

                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle successfully added",
                            Success = true
                        };
                    }

                    return new OperationResult()
                    {
                        Message = "Vehicle can't be added at the moment. Please try again later",
                        Success = false
                    };
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Vehicle can't be added at the moment. Please try again later",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult ChangeVehicle(VehicleResult vehicle)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var vehicleItem =
                        accessDb.Vehicles.FirstOrDefault(v => v.Registration.Equals(vehicle.Registration));
                    if (vehicleItem == null)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle with registration " + vehicle.Registration + " doesn't exist.",
                            Success = false
                        };
                    }

                    vehicleItem.Brand = vehicle.Brand;
                    vehicleItem.LoadCapacity = vehicle.LoadCapacity;
                    vehicleItem.ProductionYear = vehicle.ProductionYear;
                    vehicleItem.Mileage = vehicle.Mileage;
                    vehicleItem.Type = vehicle.Type;


                    var result = accessDb.SaveChanges();
                    if (result >= 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle successfully changed",
                            Success = true
                        };
                    }

                    return new OperationResult()
                    {
                        Message = "Vehicle can't be changed at the moment. Please try again later",
                        Success = false
                    };
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Vehicle can't be changed at the moment. Please try again later",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult RemoveVehicle(string registration)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var vehicleItem =
                        accessDb.Vehicles.FirstOrDefault(v => v.Registration.Equals(registration));
                    if (vehicleItem == null)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle with registration " + registration + " doesn't exist.",
                            Success = false
                        };
                    }

                    accessDb.Vehicles.Remove(vehicleItem);
                    var result = accessDb.SaveChanges();
                    if (result >= 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Vehicle successfully removed",
                            Success = true
                        };
                    }

                    return new OperationResult()
                    {
                        Message = "Vehicle can't be removed at the moment. Please try again later",
                        Success = false
                    };
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Vehicle can't be removed at the moment. Please try again later",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }
    }
}