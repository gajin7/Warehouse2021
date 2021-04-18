﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/warehouse")]
    public class WarehouseController : ApiController
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public WarehouseController()
        {

        }
        public WarehouseController(IWarehouseRepository warehouseRepository, IEmployeeRepository employeeRepository)
        {
            _warehouseRepository = warehouseRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IEnumerable<WarehouseResult> GetWarehouses()
        {
            var warehouses = _warehouseRepository.GetAllWarehouses();
            return (from item in warehouses let employee = _employeeRepository.GetUserById(item.ManagerId)
                select new WarehouseResult() {Address = item.Address, StorekeeperName = employee.FirstName + " " + employee.LastName, WarehouseId = item.Id}).ToList();
        }

    }
}