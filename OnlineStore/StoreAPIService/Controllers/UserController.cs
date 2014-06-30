using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineStore.Contracts;

namespace StoreAPIService.Controllers
{
    /// <summary>
    /// The API controller for User operations
    /// </summary>
    public class UserController : ApiController
    {
        private readonly MyCartDBEntities dbContext = new MyCartDBEntities();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController()
        {
        }

        /// <summary>
        /// Gets the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException">new HttpResponseMessage
        ///                    {
        ///                        ReasonPhrase = User does not Exists,
        ///                        StatusCode = HttpStatusCode.InternalServerError
        ///                    }</exception>
        /// <exception cref="HttpResponseMessage"></exception>
        [HttpGet]
        public UserCredentials Get(string username)
        {
            var user = dbContext.Users.FirstOrDefault(i => i.Username.Equals(username));

            if (user != null)
            {
                var users = dbContext.Users.Join(dbContext.Customers, u => u.UserId, c => c.UserId, (u, c) =>
                    new UserCredentials
                    {
                        CustomerId = c.CustomerId,
                        UserId = u.UserId,
                        UserName = u.Username,
                        CustomerAddress = c.Address,
                        CustomerName = c.CustomerName,
                        Email = c.Email,
                        Phone = c.Phone
                    }).ToList();

                if (users != null)
                {
                    return users.Find(l => l.UserName.Equals(username));
                }
            }

            throw new HttpResponseException
                   (new HttpResponseMessage
                   {
                       ReasonPhrase = "User does not Exists",
                       StatusCode = HttpStatusCode.InternalServerError
                   });
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpGet]
        public bool ValidateUser(string username, string password)
        {
            var usr = dbContext.Users.FirstOrDefault(i => i.Username.Equals(username));
            if (usr != null)
            {
                if (usr.Password.Equals(password))
                    return true;
                return false;
            }

            return false;
        }


        /// <summary>
        /// Creates the specified create user.
        /// </summary>
        /// <param name="createUser">The create user.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException">new HttpResponseMessage
        ///                     {
        ///                         ReasonPhrase = User Already Exists,
        ///                         StatusCode = HttpStatusCode.InternalServerError
        ///                     }</exception>
        /// <exception cref="HttpResponseMessage"></exception>
        [HttpPost]
        public UserCredentials Create([FromBody] UserCredentials createUser)
        {
            UserCredentials user = null;

            User usr = null;
            //if (dbContext.Users.Count() > 0)
            //{
            usr = dbContext.Users.FirstOrDefault(i => i.Username.Equals(createUser.UserName));
            if (usr != null)
                throw new HttpResponseException
                    (new HttpResponseMessage
                    {
                        ReasonPhrase = "User Already Exists",
                        StatusCode = HttpStatusCode.InternalServerError
                    });
            //}

            //if (usr == null)
            //{
            var userInsert = new User
            {
                Username = createUser.UserName,
                Password = createUser.Password
            };

            dbContext.Users.AddObject(userInsert);
            var userId = dbContext.SaveChanges();
            var customerInsert = new Customer
            {
                CustomerName = createUser.CustomerName,
                Phone = createUser.Phone,
                Email = createUser.Email,
                Address = createUser.CustomerAddress,
                UserId = userInsert.UserId
            };

            dbContext.Customers.AddObject(customerInsert);
            dbContext.SaveChanges();

            createUser.CustomerId = customerInsert.CustomerId;
            createUser.UserId = userInsert.UserId;
            user = createUser;
            return user;
            // }

            //return user;
        }
    }
}
