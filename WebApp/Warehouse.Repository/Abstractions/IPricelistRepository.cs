using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Repository.Abstractions
{
    public interface IPricelistRepository
    {
        double? GetPriceForItem(string itemId, DateTime? date = null);
    }
}
