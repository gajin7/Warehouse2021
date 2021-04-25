using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        public IEnumerable<Company> GetCompanies()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Companies.ToList();
            }
        }
    }
}