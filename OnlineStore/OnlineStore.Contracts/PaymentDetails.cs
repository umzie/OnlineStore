using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Contracts
{
    public class PaymentDetails
    {
        public int PaymentId { get; set; }
        public string DeliveryAddress { get; set; }
        public string BasketName { get; set; }
        public int BasketId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
