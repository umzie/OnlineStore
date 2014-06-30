using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;
using System.Net.Http;
using System.Net;
using OnlineStore.Common;
using OnlineStore.Contracts;
using System.Net.Http.Formatting;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        //
        // GET: /Basket/

        private readonly IBasketManager basketMgr;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketController"/> class.
        /// </summary>
        /// <param name="basketmgr">The basketmgr.</param>
        public BasketController(IBasketManager basketmgr)
        {
            this.basketMgr = basketmgr;
        }

      
   
        public ActionResult Checkout(Dictionary<string,object> model)
        {
            if (Convert.ToBoolean(model["PayLater"]))
            {
                RedirectToAction("Index", "PayLater");
            }
            if (Convert.ToBoolean(model["PayNow"]))
            { 
                RedirectToAction("Index", "PayNow"); 
            }

            return View(model);
            //return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            HomeViewModel model = null;
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int customerId = 0;

            if (Session["User"] != null)
            {
                var userInfo = Session["User"] as UserCredentials;

                customerId = userInfo.CustomerId;
            }

            var basket = this.basketMgr.GetBasket(cart.ShoppingCartId, customerId);

            if (basket.BasketName == null)
            {
                Session["TotalAmount"] = null;
                return View(model);
            }

            model = new HomeViewModel
            {
                BasketViewModel = new BasketViewModel
                {
                    CustomerId = basket.CustomerId,
                    TotalCost = Convert.ToDecimal(basket.Amount)
                },
                ItemModel = basket.BasketItems.Select(i => new ItemModel
                {
                    Id = i.ItemId,
                    ItemName = i.ItemName,
                    Image = i.ImagePath,
                    Unit = i.Unit,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToArray()

            };

            Session["TotalAmount"] = Convert.ToDecimal(basket.Amount);
            return View(model);
        }


        public ActionResult AddItem(int itemId, int quantity)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            Basket basket = new Basket();

            basket.BasketName = cart.ShoppingCartId;

            if (Session["User"] != null)
            {
                var userInfo = Session["User"] as UserCredentials;

                if (userInfo != null)
                {
                    basket.CustomerId = userInfo.CustomerId;
                }
            }

            basket.BasketItems = new[] { new Item { ProductId = itemId, Quantity = quantity } };

            this.basketMgr.AddItem(basket);

            return RedirectToAction("Index");
        }

        public ActionResult Empty()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            this.basketMgr.EmptyBasket(cart.ShoppingCartId);
            return RedirectToAction("Index");
        }


        public ActionResult AddToShoppingList(int itemId)
        {
            int customerId = 0;
            if (Session["User"] != null)
            {
                var userInfo = Session["User"] as UserCredentials;

                customerId = userInfo.CustomerId;
            }

            var res = this.basketMgr.AddToShoppingList(customerId, itemId);

            string response = res ? "Item added to Shopping List" : "Error in adding item to shopping list";

            return Content(response);
        }
    }
}
