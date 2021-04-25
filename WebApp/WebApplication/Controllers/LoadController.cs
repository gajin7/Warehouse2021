using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/load")]
    public class LoadController : ApiController
    {
        private readonly ILoadRepository _loadRepository;
        public LoadController()
        {

        }

        public LoadController(ILoadRepository loadRepository)
        {
            _loadRepository = loadRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("getUnloadedLoads")]
        public IEnumerable<LoadResult> GetUnloadedLoads()
        {
            return _loadRepository.GetWaitingLoads();
        }
    }
}