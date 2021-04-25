using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface ILoadRepository
    {
        OperationResult CreateLoad(string reportId, string storekeeperId, string receiptId, string rampId);
        IEnumerable<LoadResult> GetWaitingLoads();
    }
}
