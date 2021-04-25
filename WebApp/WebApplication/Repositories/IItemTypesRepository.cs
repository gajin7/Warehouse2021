using System.Collections.Generic;

namespace WebApplication.Repositories
{
    public interface IItemTypesRepository
    {
        IEnumerable<string> GetItemTypes();
    }
}