using System.Collections.Generic;
using Warehouse.Model;
using Warehouse.Web.Model.Response;

namespace Warehouse.Repository.Abstractions
{
    public interface ICompaniesRepository
    {
        IEnumerable<Company> GetCompanies();
        OperationResult AddCompany(CompanyResult company);
        OperationResult ChangeCompany(CompanyResult company);
        OperationResult RemoveCompany(string companyPib);
    }
}
