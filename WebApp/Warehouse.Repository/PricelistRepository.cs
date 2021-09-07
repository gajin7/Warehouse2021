using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;

namespace Warehouse.Repository
{
    public class PricelistRepository : IPricelistRepository
    {
        public double? GetPriceForItem(string itemId, DateTime? date = null)
        {
            using (var accessDb = new AccessDb())
            {
                if (date == null)
                {
                    return accessDb.Pricelists.FirstOrDefault(i =>
                        i.Id.Equals(itemId) && (i.ValidTo == null || i.ValidTo >= DateTime.Now))?.Value;
                }
                else
                {
                    return accessDb.Pricelists.FirstOrDefault(i =>
                        i.Id.Equals(itemId) && (i.ValidFrom <= date && i.ValidTo >= date))?.Value;
                }
            }
        }
    }
}
