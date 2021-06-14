using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        public ReceiptOperationResult CreateReceipt(IEnumerable<ItemResult> items, string company)
        {
            using (var accessDb = new AccessDb())
            {
                var enumerable = items.ToList();
                var amount = enumerable.Aggregate<ItemResult, double?>(0, (current, item) => current + item.Amount*item.Quantity);
                var receipt = new Receipt()
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.Now.ToLongDateString(),
                    Amount = amount,
                    Company = company,
                };
                var recepitItemsList = enumerable.Select(item => new ReceiptItem() {ItemId = item.Id, Quantity = item.Quantity, ReceiptId = receipt.Id}).ToList();
                receipt.ReceiptItems = recepitItemsList;

                try
                {
                    accessDb.Receipts.Add(receipt);
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new ReceiptOperationResult()
                        {
                            Success = true,
                            Message = "Receipt created.",
                            ReceiptId = receipt.Id,
                        };
                    }
                    else
                    {
                        return new ReceiptOperationResult()
                        {
                            Success = false,
                            Message = "Receipt can't be created at the moment."
                        };
                    }

                }
                catch (Exception e)
                {
                    return new ReceiptOperationResult()
                    {
                        Success = false,
                        Message = "Receipt can't be created at the moment.",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public IEnumerable<Receipt> GetReceipts()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Receipts.ToList();
            }
        }

        public Receipt GetReceipt(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Receipts.FirstOrDefault(r => r.Id.Equals(id));
            }
        }

        public IEnumerable<ReceiptItem> GetReceiptItems(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.ReceiptItems.Where(ri => ri.ReceiptId.Equals(id)).ToList();
            }
        }
    }
}