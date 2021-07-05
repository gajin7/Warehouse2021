using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface IReportRepository
    {
        OperationResult CreateInReport(Item item);
        ReportOperationResult CreateOutReport(IEnumerable<ItemResult> items);
        IEnumerable<ReportResult> GetReports(string reportType);
        Report GetReport(string reportId);
        IEnumerable<ItemReport> GetReportItems(string reportId);
    }
}