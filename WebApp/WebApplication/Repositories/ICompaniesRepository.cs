using System.Collections.Generic;
using WebApplication.Models;


namespace WebApplication.Repositories
{
    public interface ICompaniesRepository
    {
        IEnumerable<Company> GetCompanies();
        OperationResult AddCompany(CompanyResult company);
        OperationResult ChangeCompany(CompanyResult company);
        OperationResult RemoveCompany(string companyPib);
    }
}
