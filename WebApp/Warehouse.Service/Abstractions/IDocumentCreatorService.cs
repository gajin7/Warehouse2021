using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Service.Abstractions
{
    public interface IDocumentCreatorService
    {
        OperationResult CreateReceiptPdf(Receipt receipt, IEnumerable<ReceiptItem> receiptItems);
        OperationResult CreateReportPdf(Report report, IEnumerable<ItemReport> reportItems);

    }
}