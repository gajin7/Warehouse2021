using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/ramp")]
    public class RampController : ApiController
    {
        private readonly IRampRepository _rampRepository;

        public RampController(IRampRepository rampRepository)
        {
            _rampRepository = rampRepository;
        }

        [HttpGet]
        [Route("getRamps")]
        public IEnumerable<RampResult> GetRamps([FromUri] string warehouseId)
        {
            return _rampRepository.GetAllRamps(warehouseId);
        }

        [HttpPost]
        [Route("addRamp")]
        public OperationResult AddRamp([FromBody]RampResult ramp)
        {
            return _rampRepository.AddRamp(ramp);
        }

        [HttpPost]
        [Route("changeRamp")]
        public OperationResult ChangeRamp([FromBody]RampResult ramp)
        {
            return _rampRepository.ChangeRamp(ramp);
        }

        [HttpDelete]
        [Route("removeRamp")]
        public OperationResult RemoveRamp([FromUri]string rampId)
        {
            return _rampRepository.RemoveRamp(rampId);
        }
    }
}