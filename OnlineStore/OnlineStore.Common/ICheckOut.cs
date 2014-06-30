using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Contracts;

namespace OnlineStore.Common
{
    public interface ICheckOut
    {
        PaymentDetails Payment(string basketName, string cardNumber, string amount);
    }
}
