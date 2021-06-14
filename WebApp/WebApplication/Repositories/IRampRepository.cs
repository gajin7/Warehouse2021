
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
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