using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Controllers.Parameters;
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
        [Authorize]
        public IEnumerable<WarehouseResult> GetWarehouses()
        {
            var warehouses = _warehouseRepository.GetAllWarehouses();
            var retVal = (from item in warehouses let employee = _employeeRepository.GetUserById(item.ManagerId)
                select new WarehouseResult() {Address = item.Address, StorekeeperName = employee.FirstName + " " + employee.LastName, WarehouseId = item.Id, StorekeeperEmail = employee.Email, StorekeeperId = employee.Id}).ToList();
            return retVal;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("AddWarehouses")]
        public OperationResult AddWarehouses([FromBody]WarehouseParameters warehouse)
        {
            return _warehouseRepository.AddWarehouse(warehouse);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("ChangeWarehouses")]
        public OperationResult ChangeWarehouses([FromBody]WarehouseParameters warehouse)
        {
            return _warehouseRepository.ChangeWarehouse(warehouse);
        }

    }
}