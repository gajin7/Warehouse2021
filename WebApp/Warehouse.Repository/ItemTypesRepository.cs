using System.Collections.Generic;
using System.Linq;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;

namespace Warehouse.Repository
{
    public class ItemTypesRepository : IItemTypesRepository
    {
        public IEnumerable<string> GetItemTypes()
        {
            using (var accessDb = new AccessDb())
            {
                var itemTypes = accessDb.ItemTypes.ToList();
                return itemTypes.Select(t => t.Name).ToList();
            }
        }
    }
}