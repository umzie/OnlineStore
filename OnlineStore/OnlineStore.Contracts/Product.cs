using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Contracts
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string ImagePath { get; set; }
    }
}
