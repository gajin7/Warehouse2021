using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AccessDb _accessDb;
        public ItemRepository(AccessDb accessDb)
        {
            _accessDb = accessDb;
        }
        public OperationResult AddItem(Item item)
        {
            var result = new OperationResult();

            try
            {
                if (!_accessDb.Items.Any(e => e.Id.Equals(item.Id)))
                {
                    _accessDb.Items.Add(item);
                    var dbResult = _accessDb.SaveChanges();
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

        public OperationResult ChangeQuantity(string id, double addToQuantity)
        {
            var result = new OperationResult();

            try
            {
                if (_accessDb.Items.Any(e => e.Id.Equals(id)))
                {
                    var item =_accessDb.Items.First(e => e.Id.Equals(id));
                    if (addToQuantity < 0)
                    {
                        if ((item.Quantity - (int) addToQuantity) > 0)
                        {
                            item.Quantity -= (int) addToQuantity;
                            var dbResult = _accessDb.SaveChanges();
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
                        item.Quantity += (int) addToQuantity;
                        result.Message = "Item quantity changed successfully";
                        result.Success = true;

                        var dbResult = _accessDb.SaveChanges();
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

            return result;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _accessDb.Items.ToList();
        }

        public Item GetItem(string id)
        {
            return _accessDb.Items.FirstOrDefault(i => i.Id.Equals(id));
        }
    }
}