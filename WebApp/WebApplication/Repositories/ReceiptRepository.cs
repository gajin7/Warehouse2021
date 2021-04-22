using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly AccessDb _accessDb;
        public ReceiptRepository(AccessDb accessDb)
        {
            _accessDb = accessDb;
        }
        public OperationResult CreateReceipt(IEnumerable<Item> items, string company)
        {
            var enumerable = items.ToList();
            var amount = enumerable.Aggregate<Item, double?>(0, (current, item) => current + item.Amount);
            var receipt = new Receipt()
            {
                Date = DateTime.Now.ToLongDateString(),
                Items =  enumerable.ToList(),
                Amount = amount,
                Company = company
            };
            try
            {
                _accessDb.Receipts.Add(receipt);
                var result = _accessDb.SaveChanges();
                if (result > 0)
                {
                    return new OperationResult()
                    {
                        Success = true,
                        Message = "Receipt created."
                    };
                }
                else
                {
                    return new OperationResult()
                    {
                        Success = false,
                        Message = "Receipt can't be created at the moment."
                    };
                }
                
            }
            catch (Exception e)
            {
                return new OperationResult()
                {
                    Success = false,
                    Message = "Receipt can't be created at the moment.",
                    ErrorMessage = e.Message
                };
            }
        }
    }
}