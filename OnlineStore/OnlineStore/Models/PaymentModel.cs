using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class PaymentModel
    {
        public int CustomerId { get; set; }
        public int BasketId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public int CardNumber { get; set; }
    }
}