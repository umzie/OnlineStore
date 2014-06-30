using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Contracts
{
    public class Item
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
    }
}
