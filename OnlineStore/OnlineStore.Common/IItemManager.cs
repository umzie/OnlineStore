using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Contracts;

namespace OnlineStore.Common
{
    public interface IItemManager
    {
        Basket IncrementItem(Item incrementItem);
        Basket DecrementItem(Item decrementItem);
    }
}
