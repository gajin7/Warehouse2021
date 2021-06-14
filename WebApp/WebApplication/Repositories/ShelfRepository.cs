using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ShelfRepository : IShelfRepository
    {
        public IEnumerable<Shelf> GetShelvesInWarehouse(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var shelves = accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId)).ToList();
                foreach (var shelf in shelves)
                {
                    foreach (var item in shelf.Items)
                    {
                        if (item.Quantity != 0) continue;
                        shelf.Items.Remove(item);
                        break;
                    }
                }

                return shelves;
            }
        }

        public IEnumerable<Shelf> GetShelves(string warehouseId)
        {
            using (var accessDb = new AccessDb())
            {
                var shelves = accessDb.Shelves.Where(s => s.WarehouseId.Equals(warehouseId)).ToList();

                return shelves;
            }
        }

        public OperationResult AddShelf(string warehouseId, string shelfId)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Shelves.Any(s => s.Id.Equals(shelfId)))
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Shelf with the same name already exist, please try again with different name"
                        };
                    }
                    else
                    {
                        accessDb.Shelves.Add(new Shelf {Id = shelfId, WarehouseId = warehouseId});
                        var result = accessDb.SaveChanges();
                        if (result > 0)
                        {
                            return new OperationResult
                            {
                                Success = true,
                                Message = "Shelf successfully added"
                            };
                        }
                        else
                        {
                            return new OperationResult
                            {
                                Success = false,
                                Message = "You can't delete this shelf at the moment, please try again later"
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong, please try again",
                        ErrorMessage = e.Message
                    };

                }
            }
        }

        public OperationResult RemoveShelf(string warehouseId, string shelfId)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (!accessDb.Shelves.Any(s => s.WarehouseId.Equals(warehouseId) && s.Id.Equals(shelfId)))
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Shelf can't be deleted, please try again later"
                        };
                    }
                    else
                    {
                        var shelf = accessDb.Shelves.FirstOrDefault(s => s.Id.Equals(shelfId) && s.WarehouseId.Equals(warehouseId));
                        if (shelf?.Items.Count > 0)
                        {
                            return new OperationResult
                            {
                                Success = false,
                                Message = "Shelf can't be deleted, please remove items from shelf and try again"
                            };
                        }
                        accessDb.Shelves.Remove(shelf ?? throw new InvalidOperationException());
                        var result = accessDb.SaveChanges();
                        if (result > 0)
                        {
                            return new OperationResult
                            {
                                Success = true,
                                Message = "Shelf successfully deleted"
                            };
                        }
                        else
                        {
                            return new OperationResult
                            {
                                Success = false,
                                Message = "You can't delete this shelf at the moment, please try again later"
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong, please try again",
                        ErrorMessage = e.Message
                    };

                }
            }
        }
    }
}