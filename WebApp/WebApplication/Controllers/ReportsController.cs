using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportsController : ApiController
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDocumentCreatorService _documentCreatorService;
        public ReportsController(IDocumentCreatorService documentCreatorService, IReportRepository reportRepository)
        {
            _documentCreatorService = documentCreatorService;
            _reportRepository = reportRepository;
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

        [HttpGet]
        //[Authorize]
        [Route("getReportPdf")]
        public HttpResponseMessage GetReportPdf([FromUri] string reportId)
        {
            var report = _reportRepository.GetReport(reportId);
            var items = _reportRepository.GetReportItems(reportId);
            var result = _documentCreatorService.CreateReportPdf(report, items);

            if (!result.Success) return Request.CreateResponse(HttpStatusCode.BadRequest);


            var dataBytes = File.ReadAllBytes(result.Message);
            var dataStream = new MemoryStream(dataBytes);

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "Report" + report.Id + ".pdf"
            };
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return httpResponseMessage;

        }


    }
}