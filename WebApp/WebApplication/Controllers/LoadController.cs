using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Controllers.Parameters;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/load")]
    public class LoadController : ApiController
    {
        private readonly ILoadRepository _loadRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public LoadController()
        {

        }

        public LoadController(ILoadRepository loadRepository, IEmployeeRepository employeeRepository)
        {
            _loadRepository = loadRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("getUnloadedLoads")]
        public IEnumerable<LoadResult> GetUnloadedLoads([FromUri]string warehouseId)
        {
            return _loadRepository.GetWaitingLoads(warehouseId);
        }

        [HttpGet]
        [Authorize]
        [Route("getLoadedLoads")]
        public IEnumerable<LoadResult> GetLoadedLoads([FromUri]string warehouseId)
        {
            return _loadRepository.GetLoadedLoads(warehouseId);
        }

        [HttpGet]
        [Authorize]
        [Route("getLoadingLoads")]
        public IEnumerable<LoadResult> GetLoadingLoads([FromUri]string warehouseId)
        {
            return _loadRepository.GetLoadingLoads(warehouseId);
        }

        [HttpPost]
        [Authorize]
        [Route("takeLoadByDriver")]
        public OperationResult TakeLoadByDriver([FromBody] GetLoadByDriverParameters getLoadByDriverParameters)
        {
            var employeeId = _employeeRepository.GetIdByEmail(getLoadByDriverParameters.Email);
            return _loadRepository.TakeLoadByDriver(getLoadByDriverParameters.LoadId,employeeId);
        }

        [HttpPost]
        [Authorize]
        [Route("finishLoading")]
        public OperationResult FinishLoading([FromBody] GetLoadByDriverParameters getLoadByDriverParameters)
        {
            return _loadRepository.FinishLoading(getLoadByDriverParameters.LoadId);
        }

        [HttpGet]
        [Authorize]
        [Route("getTakenLoadForDriver")]
        public LoadResult GetTakenLoadForDriver([FromUri]string email)
        {
            var employeeId = _employeeRepository.GetIdByEmail(email);
            return _loadRepository.GetTakenLoadForDriver(employeeId);
        }
    }
}