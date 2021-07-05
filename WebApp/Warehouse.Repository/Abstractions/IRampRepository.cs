
using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IRampRepository
    {
        Ramp GetRampForLoad(string warehouseId);
        IEnumerable<RampResult> GetAllRamps(string warehouseId);
        OperationResult AddRamp(RampResult ramp);
        OperationResult ChangeRamp(RampResult ramp);
        OperationResult RemoveRamp(string id);
    }
}