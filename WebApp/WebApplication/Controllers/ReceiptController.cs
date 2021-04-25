using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Ajax.Utilities;
using WebApplication.Controllers.Parameters;
using WebApplication.Models;
using WebApplication.Repositories;

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

        public ReceiptController()
        {
        }

        public ReceiptController(IReceiptRepository receiptRepository,IItemRepository itemRepository,
            IReportRepository reportRepository,ILoadRepository loadRepository,
            IEmployeeRepository employeeRepository,IRampRepository rampRepository)
        {
            _receiptRepository = receiptRepository;
            _itemRepository = itemRepository;
            _reportRepository = reportRepository;
            _loadRepository = loadRepository;
            _employeeRepository = employeeRepository;
            _rampRepository = rampRepository;
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
        [Route("createReceipt")]
        public JsonResult<IEnumerable<Receipt>> GetReceipts()
        {
            var receipts = _receiptRepository.GetReceipts();
            return Json(receipts);
        }
    }
}