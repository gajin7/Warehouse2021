using System.Collections.Generic;

namespace Warehouse.Repository.Abstractions
{
    public interface IItemTypesRepository
    {
        IEnumerable<string> GetItemTypes();
    }
}