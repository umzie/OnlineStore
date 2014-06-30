using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Common;
using System.Net.Http;
using System.Net;
using OnlineStore.Contracts;

namespace OnlineStoreProvider
{
    public class CheckOutManager : ICheckOut
    {
        public PaymentDetails Payment(string basketName, string cardNumber, string amount)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = client.GetAsync(string.Format("http://localhost:38696/api/MyCart?basketName={0}&cardNo={1}&amount={2}", basketName, cardNumber, amount)).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var payment = response.Content.ReadAsAsync<PaymentDetails>().Result;
                return payment;
            }

            return null;
        }
    }
}
