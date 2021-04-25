using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Repositories
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