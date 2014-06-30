using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Models
{
    public class LineItemModel
    {

        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

    }
}
