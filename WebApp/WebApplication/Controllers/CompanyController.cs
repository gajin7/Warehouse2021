using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Repositories;
using WebApplication.Services;

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
        [Route("addCompany")]
        public OperationResult AddCompany([FromBody]CompanyResult company)
        {
            return _companiesRepository.AddCompany(company);
        }

        [HttpPost]
        [Route("changeCompany")]
        public OperationResult ChangeCompany([FromBody]CompanyResult company)
        {
            return _companiesRepository.ChangeCompany(company);
        }

        [HttpDelete]
        [Route("removeCompany")]
        public OperationResult RemoveCompany([FromUri]string companyPib)
        {
            return _companiesRepository.RemoveCompany(companyPib);
        }

        [Route("getAllCompaniesSearch")]
        [Authorize]
        [HttpGet]
        public IEnumerable<CompanyResult> GetAllCompaniesSearch([FromUri]string keyWord)
        {
            var companies = _companiesRepository.GetCompanies();
            return _searchService.FilterAllCompaniesBaseOnKeyWord(companies, keyWord).ToList();
        }
    }
}