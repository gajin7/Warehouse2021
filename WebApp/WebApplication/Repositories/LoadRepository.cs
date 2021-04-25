using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
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
        public IEnumerable<LoadResult> GetWaitingLoads()
        {
            using (var accessDb = new AccessDb())
            {
                var loads = accessDb.Loads.Where(l => l.VehicleId == null || l.VehicleId == "").ToList();
                return loads.Select(load => new LoadResult() { Id = load.Id, Ramp = load.RampId, Warehouse = load.Ramp.WarehouseId, RampFree = load.Ramp.Free}).ToList();
            }
        }
    }
}