using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Ajax.Utilities;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;


namespace WebApplication.Controllers
{
    [RoutePrefix("api/receipt")]
    public class ReceiptController : ApiController
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ILoadRepository _loadRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRampRepository _rampRepository;
        private readonly IDocumentCreatorService _documentCreatorService;

        public ReceiptController()
        {
        }

        public ReceiptController(IReceiptRepository receiptRepository,IItemRepository itemRepository,
            IReportRepository reportRepository,ILoadRepository loadRepository,
            IEmployeeRepository employeeRepository,IRampRepository rampRepository,
            IDocumentCreatorService documentCreatorService)
        {
            _receiptRepository = receiptRepository;
            _itemRepository = itemRepository;
            _reportRepository = reportRepository;
            _loadRepository = loadRepository;
            _employeeRepository = employeeRepository;
            _rampRepository = rampRepository;
            _documentCreatorService = documentCreatorService;
        }

        [HttpPost]
        [Authorize]
        [Route("createReceipt")]
        public OperationResult CreateReceipt([FromBody]ReceiptParameters parameters)
        {
            if (parameters == null || parameters.Company.IsNullOrWhiteSpace() || parameters.StorekeeperEmail.IsNullOrWhiteSpace() || parameters.WarehouseId.IsNullOrWhiteSpace())
            {
                return new OperationResult()
                {
                    Success = false,
                    Message = "Please check your data and try again"
                };
            }

            var employee = _employeeRepository.GetUserInfo(parameters.StorekeeperEmail);
            var receiptResult = _receiptRepository.CreateReceipt(parameters.Items, parameters.Company);
            if (receiptResult.Success)
            {
                var reportResult = _reportRepository.CreateOutReport(parameters.Items);
                if (reportResult.Success)
                {
                    var ramp = _rampRepository.GetRampForLoad(parameters.WarehouseId);
                    if (ramp == null)
                    {
                        return new OperationResult()
                        {
                            Success = false,
                            Message = "Ramp not found"
                        };
                    }
                    if (_loadRepository.CreateLoad(reportResult.ReportId, employee.Id, receiptResult.ReceiptId,ramp.Id).Success)
                    {
                        foreach (var item in parameters.Items)
                        {
                            if (item.Quantity != null)
                            {
                                var quantity = (item.Quantity * -1).Value;
                                _itemRepository.ChangeQuantity(item.Id, quantity);
                            }

                        }
                        return receiptResult;
                    }
                }
                else
                {
                    return reportResult;
                }
            }
            else
            {
                return receiptResult;
            }

            return new OperationResult()
            {
                Success = false,
                Message = "Please check your data and try again"
            };
        }

        [HttpGet]
        [Authorize]
        [Route("GetReceipts")]
        public JsonResult<IEnumerable<Receipt>> GetReceipts()
        {
            var receipts = _receiptRepository.GetReceipts();
            return Json(receipts);
        }

        [HttpGet]
        [Authorize]
        [Route("getReceiptPdf")]
        public HttpResponseMessage GetReceiptPdf([FromUri] string receiptId)
        {
            var receipt = _receiptRepository.GetReceipt(receiptId);
            var items = _receiptRepository.GetReceiptItems(receiptId);
            var result = _documentCreatorService.CreateReceiptPdf(receipt, items);

            if (!result.Success) return Request.CreateResponse(HttpStatusCode.BadRequest);


            var dataBytes = File.ReadAllBytes(result.Message);
            var dataStream = new MemoryStream(dataBytes);

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "Receipt" + receipt.Id + ".pdf"
            };
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return httpResponseMessage;

        }

    }
}