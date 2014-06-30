using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineStore.Contracts;
using System.Data;

namespace StoreAPIService.Controllers
{
    /// <summary>
    /// The API controller for MyCart operations
    /// </summary>
    public class MyCartController : ApiController
    {
        private readonly MyCartDBEntities dbContext = new MyCartDBEntities();

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCartController"/> class.
        /// </summary>
        public MyCartController()
        {
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<OnlineStore.Contracts.Product> GetAllProducts()
        {
            var products = dbContext.Products.Join(dbContext.UnitMasters, p => p.UnitId, u => u.UnitId, (p, u) => new OnlineStore.Contracts.Product
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                ImagePath = p.ImagePath,
                Unit = u.UnitName
            });

            return products.AsEnumerable();
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="addItem">The add item.</param>
        [HttpPost]
        public void AddItem([FromBody] OnlineStore.Contracts.Basket addItem)
        {
            var basketId = dbContext.Baskets.FirstOrDefault(k => k.BasketName == addItem.BasketName);
            if (basketId != null)
            {
                var bskId = dbContext.Items.Where(i => i.BasketId == basketId.BasketId).ToList();
                var getItem = addItem.BasketItems.First();
                var item = dbContext.Items.FirstOrDefault(k => k.BasketId == basketId.BasketId &&
                    k.ProductId == getItem.ProductId);
                if (item == null)
                {
                    item = new Item
                    {
                        ProductId = getItem.ProductId,
                        BasketId = basketId.BasketId,
                        Price = getItem.Price,
                        Quantity = getItem.Quantity
                    };

                    dbContext.Items.AddObject(item);
                }
                else
                {
                    item.Quantity++;
                }

                dbContext.SaveChanges();
                dbContext.AcceptAllChanges();

            }
            else
            {
                basketId = new Basket
                {
                    BasketName = addItem.BasketName,
                    CustomerId = addItem.CustomerId,
                    CreatedDateTime = DateTime.Now,
                    IsCheckedOut = false
                };
                dbContext.Baskets.AddObject(basketId);
                dbContext.SaveChanges();

                var itemToAdd = addItem.BasketItems.First();
                var item = dbContext.Products.First(p => p.ProductId == itemToAdd.ProductId);


                dbContext.Items.AddObject(new Item
                {
                    BasketId = basketId.BasketId,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = 1
                });
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the basket.
        /// </summary>
        /// <param name="basketName">Name of the basket.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public OnlineStore.Contracts.Basket GetBasket([FromUri] string basketName, [FromUri] int customerId)
        {
            var fullBasket = new OnlineStore.Contracts.Basket();

            if (!string.IsNullOrEmpty(basketName) && customerId > 0)
            {
                var b = dbContext.Baskets.Where(k => k.BasketName.Equals(basketName)).FirstOrDefault();
                if (b != null)
                {
                    var items = dbContext.Items.Where(j => j.BasketId == b.BasketId).ToList();
                    if (items.Count > 0)
                    {
                        fullBasket.BasketItems = items.Join(
                                               dbContext.Products,
                                               i => i.ProductId,
                                               p => p.ProductId,
                                               (i, p) => new OnlineStore.Contracts.Item
                                                {
                                                    ProductId = p.ProductId,
                                                    ItemId = i.ItemId,
                                                    Price = Convert.ToDecimal(p.Price),
                                                    Quantity = Convert.ToInt32(i.Quantity),
                                                    ItemName = p.ProductName,
                                                    ImagePath = p.ImagePath, //Unit=p.un
                                                }).ToArray();

                        fullBasket.BasketId = b.BasketId;
                        fullBasket.BasketName = b.BasketName;
                        fullBasket.IsCheckedOut = b.IsCheckedOut;
                        fullBasket.CustomerId = Convert.ToInt32(b.CustomerId);

                        fullBasket.Amount = Convert.ToSingle(fullBasket.BasketItems.Select(k => k.Price * k.Quantity).Sum());
                        return fullBasket;
                    }
                }
            }

            fullBasket.BasketItems = new OnlineStore.Contracts.Item[] { new OnlineStore.Contracts.Item() };
            return fullBasket;

        }


        [HttpPut]
        public void Empty([FromUri] string basketName)
        {
            var basketId = dbContext.Baskets.FirstOrDefault(k => k.BasketName == basketName);
            if (basketId != null)
            {
                var bskId = dbContext.Items.Where(i => i.BasketId == basketId.BasketId).ToList();

                if (bskId.Count > 0)
                {

                    var items = dbContext.Items.Where(k => k.BasketId == basketId.BasketId);
                    if (items != null)
                    {

                        foreach (var item in items)
                        {
                            dbContext.Items.DeleteObject(item);

                        }
                        dbContext.SaveChanges();
                        dbContext.AcceptAllChanges();
                    }
                }
            }
        }

        [HttpGet]
        public PaymentDetails CheckOut([FromUri]string basketName, [FromUri] string cardNo, [FromUri]string amount)
        {

            PaymentDetails payment = new PaymentDetails();

            long card;
            if (!string.IsNullOrWhiteSpace(cardNo) && long.TryParse(cardNo, out card))
            {

                var g = dbContext.PaymentMasters.FirstOrDefault(l => l.PaymentCardNumber == card);
                if (g != null)
                {
                    var basketId = dbContext.Baskets.FirstOrDefault(k => k.BasketName == basketName);
                    if (basketId != null)
                    {
                        basketId.TotalPrice = string.IsNullOrWhiteSpace(amount) ? 0 : Convert.ToDecimal(amount);
                        basketId.IsCheckedOut = true;
                        dbContext.SaveChanges();
                        dbContext.AcceptAllChanges();

                        Payment pay = new Payment();
                        pay.BasketId = basketId.BasketId;
                        pay.CustomerId = basketId.CustomerId;
                        pay.PaymentCard = Convert.ToInt32(card);
                        pay.PaymentDateTime = DateTime.Now;

                        dbContext.Payments.AddObject(pay);
                        dbContext.SaveChanges();
                        dbContext.AcceptAllChanges();


                        payment.BasketId = basketId.BasketId;
                        payment.BasketName = basketId.BasketName;
                        payment.PaymentId = pay.PaymentId;
                        payment.DeliveryAddress = (from c in dbContext.Customers
                                                   where c.CustomerId == basketId.CustomerId
                                                   select c.Address).FirstOrDefault();
                        payment.IsSuccess = true;
                        return payment;
                    }
                }
                return payment;
            }

            return payment;
        }


        [HttpGet]
        public bool AddToShoppingList([FromUri] int customerId, [FromUri] int productId)
        {
            if (customerId == 0 || productId == 0)
                return false;

            var get = dbContext.ShoppingLists.FirstOrDefault(j => j.CustomerId == customerId && j.ItemId == productId);
            if (get == null)
            {
                dbContext.ShoppingLists.AddObject(new ShoppingList { CustomerId = customerId, ItemId = productId });
                dbContext.SaveChanges();
            }

            return true;
        }
    }
}