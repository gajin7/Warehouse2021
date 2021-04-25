using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/vehicle")]
    public class VehicleController : ApiController
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public VehicleController()
        {

        }

        public VehicleController(IVehicleRepository vehicleRepository,IEmployeeRepository employeeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("getDriversVehicle")]
        public VehicleResult GetDriversVehicle([FromUri] string driversEmail)
        {
            var employee = _employeeRepository.GetUserInfo(driversEmail);

            var vehicle = _vehicleRepository.GetVehicleForDriver(employee.Id);
            if (vehicle != null)
            {
                return new VehicleResult()
                {
                    Registration = vehicle.Registration,
                    Brand = vehicle.Brand,
                    LoadCapacity = vehicle.LoadCapacity,
                    Mileage = vehicle.Mileage,
                    ProductionYear = vehicle.ProductionYear,
                    Type = vehicle.Type
                };
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("getFreeVehicles")]
        public IEnumerable<VehicleResult> GetFreeVehicles()
        {
            var vehicles = _vehicleRepository.GetAllFreeVehicles();
            return vehicles.Select(vehicle => new VehicleResult()
                {
                    Registration = vehicle.Registration,
                    Brand = vehicle.Brand,
                    LoadCapacity = vehicle.LoadCapacity,
                    Mileage = vehicle.Mileage,
                    ProductionYear = vehicle.ProductionYear,
                    Type = vehicle.Type
                })
                .ToList();
        }

        [HttpGet]
        [Authorize]
        [Route("giveVehicleToDriver")]
        public OperationResult GiveVehicleToDriver(string registration, string driversEmail)
        {
            if (registration.IsNullOrWhiteSpace() || driversEmail.IsNullOrWhiteSpace())
            {
               
                return new OperationResult()
                {
                    Message = "Something went wrong, please try again",
                    Success = false
                };
            }
            var employee = _employeeRepository.GetUserInfo(driversEmail);
            return _vehicleRepository.TakeVehicleByDriver(registration, employee.Id);
        }

        [HttpGet]
        [Authorize]
        [Route("freeVehicle")]
        public OperationResult FreeVehicle(string registration)
        {
            if (registration.IsNullOrWhiteSpace())
            {

                return new OperationResult()
                {
                    Message = "Something went wrong, please try again",
                    Success = false
                };
            }
            return _vehicleRepository.FreeVehicle(registration);
        }
    }
}