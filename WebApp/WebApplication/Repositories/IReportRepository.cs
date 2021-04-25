using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IReportRepository
    {
        OperationResult CreateInReport(Item item);
        ReportOperationResult CreateOutReport(IEnumerable<ItemResult> items);
        IEnumerable<ReportResult> GetReports(string reportType);
    }
}