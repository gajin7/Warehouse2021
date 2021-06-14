using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class RampRepository : IRampRepository
    {
        public OperationResult AddRamp(RampResult ramp)
        {
            if (!AssertRamp(ramp))
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
                    if (accessDb.Ramps.Any(c => c.Id.Equals(ramp.Id)))
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Ramp with same Id already exist. Please try again"
                        };
                    }

                    var freeValid = bool.TryParse(ramp.Free, out var free);
                    accessDb.Ramps.Add(new Ramp
                    {
                        Id = ramp.Id,
                        Free = freeValid && free,
                        WarehouseId = ramp.WarehouseId
                    });
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Ramp successfully added"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again"
                        };
                    }

                }

                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult ChangeRamp(RampResult ramp)
        {
            if (!AssertRamp(ramp))
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
                    var rmp = accessDb.Ramps.FirstOrDefault(c => c.Id.Equals(ramp.Id));
                    if (rmp == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Ramp can't be found. Please try again"
                        };
                    }

                    rmp.Free = rmp.Free;
                    var result = accessDb.SaveChanges();
                    if (result >= 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Ramp successfully changed"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again"
                        };
                    }

                }

                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public IEnumerable<RampResult> GetAllRamps(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var ramps =  accessDb.Ramps.Where(r => r.WarehouseId.Equals(warehouseId)).ToList();

                return ramps.Select(ramp => new RampResult {Id = ramp.Id, Free = ramp.Free.ToString(), Load = ramp.Loads.FirstOrDefault()?.Id, WarehouseId = ramp.WarehouseId}).ToList();
            }
        }

        public Ramp GetRampForLoad(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var ramp =  accessDb.Ramps.FirstOrDefault(r => r.WarehouseId.Equals(warehouseId) && r.Free) ?? accessDb.Ramps.FirstOrDefault(r => r.WarehouseId.Equals(warehouseId));
                if (ramp != null) ramp.Free = false;
                accessDb.SaveChanges();
                return ramp;
            }
        }

        public OperationResult RemoveRamp(string id)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var ramp = accessDb.Ramps.FirstOrDefault(c => c.Id.Equals(id));
                    if (ramp == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Ramp can't be found"
                        };
                    }

                    accessDb.Ramps.Remove(ramp);
                    var result = accessDb.SaveChanges();

                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Ramp successfully removed"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again",
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public bool AssertRamp(RampResult company)
        {
            return !company.Id.IsNullOrWhiteSpace() && !company.WarehouseId.IsNullOrWhiteSpace() && company.Free != null;

        }
    }
}