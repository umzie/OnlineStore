using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string  ImagePath { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }

    }
}
