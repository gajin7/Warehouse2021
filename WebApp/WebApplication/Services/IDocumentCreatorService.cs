using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IDocumentCreatorService
    {
        OperationResult CreateReceiptPdf(Receipt receipt, IEnumerable<ReceiptItem> receiptItems);

    }
}