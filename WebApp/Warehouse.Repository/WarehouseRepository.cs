using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        public IEnumerable<Model.Warehouse> GetAllWarehouses()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Warehouses.ToList();
            }
        }

        
        public Model.Warehouse GetWarehouse(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Warehouses.FirstOrDefault(i => i.Id.Equals(id));
            }
        }

        public OperationResult AddWarehouse(WarehouseParameters warehouse)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var employee = accessDb.Employees.FirstOrDefault(e => e.Email.Equals(warehouse.StorekeeperEmail));
                    var w = new Model.Warehouse
                    {
                        Address = warehouse.Address,
                        Id = warehouse.WarehouseId,
                        ManagerId = employee?.Id
                    };
                    accessDb.Warehouses.Add(w);
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Warehouse added"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Warehouse can't be added at this moment. Please try again later"
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
                
            }
        }

        public OperationResult ChangeWarehouse(WarehouseParameters warehouse)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var war = accessDb.Warehouses.FirstOrDefault(w => w.Id.Equals(warehouse.WarehouseId));
                    if (war == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Warehouse can't be changed at this moment. Please try again later"
                        };
                    }

                    var employee = accessDb.Employees.FirstOrDefault(e => e.Email.Equals(warehouse.StorekeeperEmail));

                    war.Id = warehouse.WarehouseId;
                    war.Address = warehouse.Address;
                    war.ManagerId = employee?.Id;
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Warehouse changed"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Warehouse can't be changed at this moment. Please try again later"
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }

            }
        }
    }
}