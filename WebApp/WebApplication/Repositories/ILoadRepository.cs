using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface ILoadRepository
    {
        OperationResult CreateLoad(string reportId, string storekeeperId, string receiptId, string rampId);
        IEnumerable<LoadResult> GetWaitingLoads(string warehouseId);
        IEnumerable<LoadResult> GetLoadedLoads(string warehouseId);
        IEnumerable<LoadResult> GetLoadingLoads(string warehouseId);
        IEnumerable<LoadResult> GetAllLoads();
        OperationResult TakeLoadByDriver(string id, string driverId);
        LoadResult GetTakenLoadForDriver(string driverId);
        OperationResult FinishLoading(string id);
    }
}
