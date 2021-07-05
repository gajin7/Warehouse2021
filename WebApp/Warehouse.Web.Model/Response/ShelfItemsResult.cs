using System.Collections.Generic;


namespace Warehouse.Web.Model.Response
{
    public class ShelfItemsResult
    {
        public string Name { get; set; }
        public IEnumerable<ItemResult> Items { get; set; }
    }
}