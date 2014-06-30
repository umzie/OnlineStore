using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Common;
using OnlineStore.Contracts;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace OnlineStoreProvider
{
    public class BasketManager : IBasketManager
    {

        public void AddItem(Basket addItem)
        {
            HttpClient client = new HttpClient();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<Basket>(addItem, formatter);
            var response = client.PostAsync(string.Format("http://localhost:38696/api/MyCart"), content).Result;

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public void EmptyBasket(string basketName)
        {
            HttpClient client = new HttpClient();
            var response = client.PutAsync(string.Format("http://localhost:38696/api/MyCart/?basketName={0}", basketName), null).Result;

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public Basket GetAllItems()
        {
            throw new NotImplementedException();
        }

        public Basket GetItem(int ItemId)
        {
            throw new NotImplementedException();
        }


        public Basket GetBasket(string basketName, int customerId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = client.GetAsync(string.Format("http://localhost:38696/api/MyCart?basketName={0}&customerId={1}", basketName, customerId)).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var basket = response.Content.ReadAsAsync<OnlineStore.Contracts.Basket>().Result;
                return basket;
            }

            return null;
        }

        public bool AddToShoppingList(int customerId, int itemId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = client.GetAsync(string.Format("http://localhost:38696/api/MyCart?customerId={0}&productId={1}", customerId, itemId)).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var basket = response.Content.ReadAsAsync<bool>().Result;
                return basket;
            }

            return false;
        }
    }
}
