using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Contracts
{
    public class Basket
    {
        public int BasketId { get; set; }
        public string BasketName { get; set; }
        public Item[] BasketItems { get; set; }
        //public DeliveryDetails Delivery { get; set; }
        public float? Amount { get; set; }
        public int CustomerId { get; set; }
        public bool? IsCheckedOut { get; set; }
    }
}
