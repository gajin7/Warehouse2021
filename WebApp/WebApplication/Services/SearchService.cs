using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class SearchService : ISearchService
    {
        public IEnumerable<ShelfItemsResult> FilterShelvesItemsBaseOnKeyWord(IEnumerable<ShelfItemsResult> shelves, string keyWord)
        {
            var filterShelvesItemsBaseOnKeyWord = shelves.ToList();
            foreach (var shelf in filterShelvesItemsBaseOnKeyWord)
            {
                shelf.Items = shelf.Items.Where(i => i.Name.Contains(keyWord) || i.Type.Contains(keyWord)).ToList();
            }

            return filterShelvesItemsBaseOnKeyWord;
        }

        public IEnumerable<ItemResult> FilterAllItemsBaseOnKeyWord(IEnumerable<ItemResult> items, string keyWord)
        {
            var filterAllItemsBaseOnKeyWord = items.ToList();

            filterAllItemsBaseOnKeyWord = filterAllItemsBaseOnKeyWord.Where(i => i.Name.Contains(keyWord) || i.Type.Contains(keyWord) || i.Warehouse.Contains(keyWord)).ToList();
            
            return filterAllItemsBaseOnKeyWord;
        }
    }
}