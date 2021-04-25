using System.Collections.Generic;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportsController : ApiController
    {
        private readonly IReportRepository _reportRepository;
        public ReportsController()
        {

        }

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        [Route("getReports")]
        [Authorize]
        public IEnumerable<ReportResult> GetReports([FromUri] string type)
        {
            var reports = _reportRepository.GetReports(type);
            return reports;
        }


    }
}