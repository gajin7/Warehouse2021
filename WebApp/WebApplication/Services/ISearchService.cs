using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ISearchService
    {
        IEnumerable<ShelfItemsResult> FilterShelvesItemsBaseOnKeyWord(IEnumerable<ShelfItemsResult> shelves, string keyWord);
        IEnumerable<ItemResult> FilterAllItemsBaseOnKeyWord(IEnumerable<ItemResult> items, string keyWord);
    }
}