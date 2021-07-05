using System.Collections.Generic;
using System.Linq;
using Warehouse.Model;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Response;

namespace Warehouse.Service
{
    public class SearchService : ISearchService
    {
        public IEnumerable<ShelfItemsResult> FilterShelvesItemsBaseOnKeyWord(IEnumerable<ShelfItemsResult> shelves, string keyWord)
        {
            var filterShelvesItemsBaseOnKeyWord = shelves.ToList();
            foreach (var shelf in filterShelvesItemsBaseOnKeyWord)
            {
                shelf.Items = shelf.Items.Where(i => i.Name.ToLower().Contains(keyWord.ToLower()) || i.Type.ToLower().Contains(keyWord.ToLower()) || i.Id.ToLower().Equals(keyWord.ToLower())).ToList();
            }

            return filterShelvesItemsBaseOnKeyWord;
        }

        public IEnumerable<ItemResult> FilterAllItemsBaseOnKeyWord(IEnumerable<ItemResult> items, string keyWord)
        {
            var filterAllItemsBaseOnKeyWord = items.ToList();

            filterAllItemsBaseOnKeyWord = filterAllItemsBaseOnKeyWord.Where(i => i.Name.ToLower().Contains(keyWord.ToLower()) || i.Type.ToLower().Contains(keyWord.ToLower()) || i.Warehouse.ToLower().Contains(keyWord.ToLower()) || i.Id.ToLower().Equals(keyWord.ToLower())).ToList();
            
            return filterAllItemsBaseOnKeyWord;
        }

        public IEnumerable<EmployeeResult> FilterAllEmployeesBaseOnKeyWord(IEnumerable<EmployeeResult> employees,
            string keyWord)
        {
            var filterAllItemsBaseOnKeyWord = employees.ToList();
            filterAllItemsBaseOnKeyWord = filterAllItemsBaseOnKeyWord.Where(i => i.FirstName.ToLower().Contains(keyWord.ToLower()) || i.LastName.ToLower().Contains(keyWord.ToLower()) || i.Email.ToLower().Contains(keyWord.ToLower()) || i.Type.ToLower().Equals(keyWord.ToLower())).ToList();
            return filterAllItemsBaseOnKeyWord;
        }

        public IEnumerable<CompanyResult> FilterAllCompaniesBaseOnKeyWord(IEnumerable<Company> companies, string keyWord)
        {
            var filterAllItemsBaseOnKeyWord = companies.ToList();

            filterAllItemsBaseOnKeyWord = filterAllItemsBaseOnKeyWord.Where(i => i.Name.ToLower().Contains(keyWord.ToLower()) || i.PIB.ToLower().Contains(keyWord.ToLower()) || i.Name.ToLower().Contains(keyWord.ToLower())).ToList();

            return filterAllItemsBaseOnKeyWord.Select(comp => new CompanyResult
                {
                    AccountNo = comp.AccountNo,
                    Address = comp.Address,
                    Deposit = comp.Deposit.ToString(),
                    Name = comp.Name,
                    PIB = comp.PIB
                })
                .ToList();
        }

        public IEnumerable<LoadResult> FilterLoadById(IEnumerable<LoadResult> loads, string keyWord)
        {
            if (string.IsNullOrWhiteSpace(keyWord))
            {
                return new List<LoadResult>();
            }
            var filterAllItemsBaseOnKeyWord = loads.ToList();
            filterAllItemsBaseOnKeyWord = filterAllItemsBaseOnKeyWord.Where(i => i.Id.ToLower().Equals(keyWord.ToLower())).ToList();
            return filterAllItemsBaseOnKeyWord;
        }
    }
}