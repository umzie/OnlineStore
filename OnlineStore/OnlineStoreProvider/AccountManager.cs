using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Common;
using OnlineStore.Contracts;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net;
using System.Net.Http.Headers;

namespace OnlineStoreProvider
{
//Test comment
// Umzie comments on Dev branch
    public class AccountManager : IAccountManager
    {
        public AccountManager()
        {

        }


        public string ValidateUser(string username, string password)
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(string.Format("http://localhost:87/Service/api/User?username={0}&password={1}", username, password)).Result;

            return response;
        }

        public UserCredentials Register(UserCredentials register)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<UserCredentials>(register, formatter);
            var response = client.PostAsync("http://localhost:87/Service/api/User", content).Result;
            var formatters = new List<MediaTypeFormatter>() 
            {
                new JsonMediaTypeFormatter(),
                new XmlMediaTypeFormatter()
            };
            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadAsAsync<UserCredentials>(formatters).Result;
                if (user != null)
                    return user;
            }
            return null;
        }

        public UserCredentials GetUserInfo(string username)
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync(string.Format("http://localhost:87/Service/api/User?username={0}", username)).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var user = response.Content.ReadAsAsync<OnlineStore.Contracts.UserCredentials>().Result;
                return user;
            }

            return null;
        }
    }
}
