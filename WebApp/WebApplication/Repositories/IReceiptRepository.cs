using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IReceiptRepository
    {
        ReceiptOperationResult CreateReceipt(IEnumerable<ItemResult> items, string company);
        IEnumerable<Receipt> GetReceipts();
    }
}
