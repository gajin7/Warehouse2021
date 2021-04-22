using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IReceiptRepository
    {
        OperationResult CreateReceipt(IEnumerable<Item> items, string company);
    }
}
