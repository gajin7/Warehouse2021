using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository
{
    public class LoadRepository : ILoadRepository
    {
        public OperationResult CreateLoad(string reportId, string storekeeperId, string receiptId, string rampId)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    accessDb.Loads.Add(new Load()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ReportId = reportId,
                        StorekeeperId = storekeeperId,
                        Loaded = false,
                        RecepitId = receiptId,
                        RampId = rampId

                    });
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult()
                        {
                            Message = "Load created successfully",
                            Success = true
                        };
                    }
                    else
                    {
                        return new OperationResult()
                        {
                            Message = "Error while creating Load ",
                            Success = false
                        };
                    }
                }

                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Error while creating Load ",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }

            }

        }

        public IEnumerable<LoadResult> GetWaitingLoads(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                if (warehouseId.IsNullOrWhiteSpace() || warehouseId.Equals("all"))
                {
                    var loads = accessDb.Loads.Where(l => l.VehicleId == null || l.VehicleId == "").ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }
                else
                {
                    var loads = accessDb.Loads.Where(l =>
                        (l.VehicleId == null || l.VehicleId == "") && l.Ramp.WarehouseId.Equals(warehouseId)).ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }

            }
        }

        public IEnumerable<LoadResult> GetLoadedLoads(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                if (warehouseId.IsNullOrWhiteSpace() || warehouseId.Equals("all"))
                {
                    var loads = accessDb.Loads.Where(l => l.Loaded).ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        Vehicle = load.Vehicle.Registration,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }
                else
                {
                    var loads = accessDb.Loads.Where(l => l.Loaded && l.Ramp.WarehouseId.Equals(warehouseId)).ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        Vehicle = load.Vehicle?.Registration,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }


            }
        }

        public IEnumerable<LoadResult> GetLoadingLoads(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                if (warehouseId.IsNullOrWhiteSpace() || warehouseId.Equals("all"))
                {
                    var loads = accessDb.Loads.Where(l => l.VehicleId != null && l.VehicleId != "" && !l.Loaded)
                        .ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        Driver = load.Vehicle?.Employee?.FirstName + " " + load.Vehicle?.Employee?.LastName,
                        Vehicle = load.Vehicle?.Registration,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }
                else
                {
                    var loads = accessDb.Loads.Where(l =>
                            l.VehicleId != null && l.VehicleId != "" && !l.Loaded &&
                            l.Ramp.WarehouseId.Equals(warehouseId))
                        .ToList();
                    return loads.Select(load => new LoadResult()
                    {
                        Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        Driver = load.Vehicle.Employee.FirstName + " " + load.Vehicle.Employee.LastName,
                        Vehicle = load.Vehicle.Registration,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    }).ToList();
                }
            }
        }

        public LoadResult GetTakenLoadForDriver(string driverId)
        {
            if (driverId.IsNullOrWhiteSpace())
            {
                return null;
            }

            using (var accessDb = new AccessDb())
            {
                try
                {
                    var load = accessDb.Loads.FirstOrDefault(l => l.Vehicle.DriverId.Equals(driverId) && !l.Loaded);
                    if (load == null)
                    {
                        return null;
                    }

                    return new LoadResult
                    {
                        Id = load.Id,
                        Ramp = load.RampId,
                        RampFree = load.Ramp.Free,
                        Warehouse = load.Ramp.WarehouseId,
                        Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                        Driver = load.Vehicle.Employee.FirstName + " " + load.Vehicle.Employee.LastName,
                        Vehicle = load.Vehicle.Registration,
                        ReportId = load.ReportId,
                        ReceiptId = load.RecepitId
                    };

                }
                catch (Exception)
                {
                    return null;
                }


            }
        }



        public OperationResult TakeLoadByDriver(string id, string driverId)
        {
            if (id.IsNullOrWhiteSpace() || driverId.IsNullOrWhiteSpace())
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Please check your data and try again"
                };
            }

            using (var accessDb = new AccessDb())
            {
                try
                {
                    var load = accessDb.Loads.FirstOrDefault(l => l.Id.Equals(id));
                    if (load == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Load can't be found, please try again"
                        };
                    }

                    var vehicle = accessDb.Vehicles.FirstOrDefault(v => v.DriverId.Equals(driverId));
                    if (vehicle == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Vehicle can't be found, please try again"
                        };
                    }

                    load.VehicleId = vehicle.Registration;
                    load.Ramp.Free = false;
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Loaded"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Change can't be save, please try again"
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong, please try again",
                        ErrorMessage = e.Message
                    };
                }


            }
        }

        public OperationResult FinishLoading(string id)
        {
            if (id.IsNullOrWhiteSpace())
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Please check your data and try again"
                };
            }

            using (var accessDb = new AccessDb())
            {
                try
                {
                    var load = accessDb.Loads.FirstOrDefault(l => l.Id.Equals(id));
                    if (load == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Load can't be found, please try again"
                        };
                    }

                    load.Ramp.Free = true;
                    load.Loaded = true;
                    var result = accessDb.SaveChanges();
                    if (result >= 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Load finished, you can go"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Change can't be save, please try again"
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong, please try again",
                        ErrorMessage = e.Message
                    };
                }


            }
        }

        public IEnumerable<LoadResult> GetAllLoads()
        {
            using (var accessDb = new AccessDb())
            {
                var loads = accessDb.Loads.ToList();
                return loads.Select(load => new LoadResult()
                {
                    Id = load.Id,
                    Ramp = load.RampId,
                    Warehouse = load.Ramp.WarehouseId,
                    RampFree = load.Ramp.Free,
                    Storekeeper = load.Employee.FirstName + " " + load.Employee.LastName,
                    Driver = load.Vehicle?.Employee?.FirstName + " " + load.Vehicle?.Employee?.LastName,
                    Vehicle = load.Vehicle?.Registration,
                    ReportId = load.ReportId,
                    ReceiptId = load.RecepitId
                }).ToList();
            }
        }
    }
}