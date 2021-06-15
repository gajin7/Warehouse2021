using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ISearchService
    {
        IEnumerable<ShelfItemsResult> FilterShelvesItemsBaseOnKeyWord(IEnumerable<ShelfItemsResult> shelves, string keyWord);
        IEnumerable<ItemResult> FilterAllItemsBaseOnKeyWord(IEnumerable<ItemResult> items, string keyWord);

        IEnumerable<EmployeeResult> FilterAllEmployeesBaseOnKeyWord(IEnumerable<EmployeeResult> employees,
            string keyWord);

        IEnumerable<CompanyResult>
            FilterAllCompaniesBaseOnKeyWord(IEnumerable<Company> companies, string keyWord);

        IEnumerable<LoadResult>
            FilterLoadById(IEnumerable<LoadResult> loads,string keyWord);
    }
}