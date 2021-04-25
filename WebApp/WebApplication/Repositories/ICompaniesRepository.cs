using System.Collections.Generic;


namespace WebApplication.Repositories
{
    public interface ICompaniesRepository
    {
        IEnumerable<Company> GetCompanies();
    }
}
