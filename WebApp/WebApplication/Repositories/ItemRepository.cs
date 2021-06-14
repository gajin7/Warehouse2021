using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ItemRepository : IItemRepository
    {

        public OperationResult AddItem(Item item)
        {
            using (var accessDb = new AccessDb())
            {
                var result = new OperationResult();
                if (item.Id.IsNullOrWhiteSpace())
                {
                    item.Id = Guid.NewGuid().ToString();
                }

                try
                {
                    if (!accessDb.Items.Any(e => e.Id.Equals(item.Id)))
                    {
                        accessDb.Items.Add(item);
                        var dbResult = accessDb.SaveChanges();
                        if (dbResult > 0)
                        {
                            result.Message = "Item added successfully";
                            result.Success = true;
                        }
                        else
                        {
                            result.Message = "Something went wrong. Please try again";
                            result.Success = false;
                        }
                    }
                    else
                    {
                        result.Message = "Item with same id already exist";
                        result.Success = false;
                    }
                }
                catch (Exception e)
                {
                    result.Message = "An error occurred. Check your data and try again";
                    result.ErrorMessage = e.Message;
                    result.Success = false;
                }

                return result;
            }
        }

        public OperationResult ChangeQuantity(string id, int addToQuantity)
        {
            var result = new OperationResult();

            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Items.Any(e => e.Id.Equals(id)))
                    {
                        var item = accessDb.Items.First(e => e.Id.Equals(id));
                        if (addToQuantity < 0)
                        {
                            if ((item.Quantity -  addToQuantity) > 0)
                            {
                                var addToQuantityAbs =  addToQuantity;
                                item.Quantity += addToQuantityAbs;
                                var dbResult = accessDb.SaveChanges();
                                if (dbResult > 0)
                                {
                                    result.Message = "Item quantity changed successfully";
                                    result.Success = true;
                                }
                                else
                                {
                                    result.Message = "Item quantity can't be changed at the moment";
                                    result.Success = false;
                                }
                            }
                            else
                            {
                                result.Message = "Item quantity can't be changed at the moment";
                                result.Success = false;
                            }

                        }
                        else
                        {
                            item.Quantity +=  addToQuantity;
                            result.Message = "Item quantity changed successfully";
                            result.Success = true;

                            var dbResult = accessDb.SaveChanges();
                            if (dbResult > 0)
                            {
                                result.Message = "Item quantity changed successfully";
                                result.Success = true;
                            }
                            else
                            {
                                result.Message = "Item quantity can't be changed at the moment";
                                result.Success = false;
                            }
                        }
                    }
                    else
                    {
                        result.Message = "Item with same id already exist";
                        result.Success = false;
                    }
                }
                catch (Exception e)
                {
                    result.Message = "An error occurred. Check your data and try again";
                    result.ErrorMessage = e.Message;
                    result.Success = false;
                }
            }

            return result;
        }

        public IEnumerable<ItemResult> GetAllItems()
        {
            using (var accessDb = new AccessDb())
            {
                var items = accessDb.Items.ToList();

               return items.Select(item => new ItemResult()
                    {
                        Amount = item.Amount,
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Type = item.Type,
                        Warehouse = item.Shelf.WarehouseId,
                        ShelfId = item.ShelfId
                    })
                    .ToList();
            }
        }

        public Item GetItem(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Items.FirstOrDefault(i => i.Id.Equals(id));
            }
        }

        public int GetQuantityForItem(string id)
        {
            using (var accessDb = new AccessDb())
            {
                var quantity = accessDb.Items.First(i => i.Id.Equals(id)).Quantity;

                if (quantity != null)
                {
                    return (int) quantity;
                }
                else
                {
                    return 0;
                }
            }
        }

        public OperationResult ChangeItem(Item item)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var oldItem = accessDb.Items.FirstOrDefault(i => i.Id.Equals(item.Id));
                    if (oldItem == null)
                    {
                        return new OperationResult
                        {
                            Message = "Item not found. Please try again.",
                            Success = false
                        };
                    }

                    oldItem.Quantity = item.Quantity;
                    oldItem.Type = item.Type;
                    oldItem.Amount = item.Amount;
                    oldItem.Name = item.Name;
                    oldItem.ShelfId = item.ShelfId;
                    var dbResult = accessDb.SaveChanges();
                    if (dbResult > 0)
                    {
                        return new OperationResult
                        {
                            Message = "Item successfully changed.",
                            Success = true
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Message = "An error occurred. Check your data and try again",
                            Success = false
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Message = "An error occurred. Check your data and try again",
                        ErrorMessage = e.Message,
                        Success = false
                    };
                }
            }
        }

        public OperationResult RemoveItem(string id)
        {
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Items.Any(e => e.Id.Equals(id)))
                    {
                        var item = accessDb.Items.First(u => u.Id == id);

                        if (item.ItemReports.Count != 0)
                        {
                            item.ItemReports.Select(i => accessDb.ItemReports.Remove(i));
                        }
                        accessDb.Items.Remove(item);
                        var dbResult = accessDb.SaveChanges();
                        if (dbResult > 0)
                        {
                            result.Message = "Item removed.";
                            result.Success = true;
                        }

                        else
                        {
                            result.Message = "Something went wrong. Please try again";
                            result.Success = false;
                        }
                    }

                    else
                    {
                        result.Message = "Item" + " can't be removed";
                        result.Success = false;
                    }

                }
                catch (Exception e)
                {
                    result.Message = "An error occurred. Check your data and try again";
                    result.ErrorMessage = e.Message;
                    result.Success = false;
                }
            }

            return result;
        }
    }
}