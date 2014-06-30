using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class BasketViewModel
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalCost { get; set; }
        public List<LineItemModel> BasketLineItems { get; set; }
       

    }
}