using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Contracts;

namespace OnlineStore.Common
{
    //Another one of my comments
    public interface IBasketManager
    {
        void AddItem(Basket addItem);
        void EmptyBasket(string basketName);
        Basket GetAllItems();
        Basket GetItem(int ItemId);
        Basket GetBasket(string basketName, int customerId);
        bool AddToShoppingList(int customerId, int itemId);
    }
}
