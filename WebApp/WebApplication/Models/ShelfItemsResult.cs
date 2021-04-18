using System.Collections.Generic;


namespace WebApplication.Models
{
    public class ShelfItemsResult
    {
        public string Name { get; set; }
        public IEnumerable<ItemResult> Items { get; set; }
    }
}