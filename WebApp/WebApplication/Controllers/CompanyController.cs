using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Response;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly ISearchService _searchService;

        public CompanyController(ICompaniesRepository companiesRepository,ISearchService searchService)
        {
            _companiesRepository = companiesRepository;
            _searchService = searchService;
        }

        [HttpGet]
        [Authorize(Roles = "admin,storekeeper")]
        [Route("getCompanies")]
        public IEnumerable<CompanyResult> GetCompanies()
        {
            return _companiesRepository.GetCompanies().ToList().Select(c => new CompanyResult()
            {
                AccountNo = c.AccountNo,
                Address = c.Address,
                Name = c.Name,
                PIB = c.PIB,
                Deposit = c.Deposit.ToString()
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("addCompany")]
        public OperationResult AddCompany([FromBody]CompanyResult company)
        {
            return _companiesRepository.AddCompany(company);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("changeCompany")]
        public OperationResult ChangeCompany([FromBody]CompanyResult company)
        {
            return _companiesRepository.ChangeCompany(company);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("removeCompany")]
        public OperationResult RemoveCompany([FromUri]string companyPib)
        {
            return _companiesRepository.RemoveCompany(companyPib);
        }

        [Route("getAllCompaniesSearch")]
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IEnumerable<CompanyResult> GetAllCompaniesSearch([FromUri]string keyWord)
        {
            var companies = _companiesRepository.GetCompanies();
            return _searchService.FilterAllCompaniesBaseOnKeyWord(companies, keyWord).ToList();
        }
    }
}