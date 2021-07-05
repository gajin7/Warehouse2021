using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;


namespace WebApplication.Controllers
{
    [RoutePrefix("api/load")]
    public class LoadController : ApiController
    {
        private readonly ILoadRepository _loadRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISearchService _searchService;
        public LoadController()
        {

        }

        public LoadController(ILoadRepository loadRepository, IEmployeeRepository employeeRepository,ISearchService searchService)
        {
            _loadRepository = loadRepository;
            _employeeRepository = employeeRepository;
            _searchService = searchService;
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

        [HttpGet]
        [Authorize]
        [Route("getLoadsById")]
        public IEnumerable<LoadResult> GetLoadsById([FromUri]string loadId)
        {
            var loads = _loadRepository.GetAllLoads();

            return _searchService.FilterLoadById(loads,loadId);
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