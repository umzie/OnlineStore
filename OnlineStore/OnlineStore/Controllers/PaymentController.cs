using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Common;
using System.Threading;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ICheckOut checkoutManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="checkout">The checkout.</param>
        public PaymentController(ICheckOut checkout)
        {
            this.checkoutManager = checkout;
        }

        //
        // GET: /Payment/

        public ActionResult Index(string totalAmount)
        {
            if (!string.IsNullOrWhiteSpace(totalAmount))
            {

            }
            return View();
        }


        public ActionResult CheckOut(string cardNo)
        {
            var amount = Session["TotalAmount"] ?? string.Empty;
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var ischeckedout = this.checkoutManager.Payment(cart.ShoppingCartId, cardNo, amount.ToString());

            string response = string.Empty;

            if (ischeckedout != null && ischeckedout.IsSuccess)
            {
                response = string.Format("Thank you {0}.This is to inform you that your order has been confirmed and will be delivered at the below address {1}", Thread.CurrentPrincipal.Identity.Name, ischeckedout.DeliveryAddress);
                Session[ShoppingCart.CartSessionKey] = null;
            }
            else
            {
                response = string.Format("Sorry {0}. We are unable to confirm your order. Please check your card number or contact us @ {1}", Thread.CurrentPrincipal.Identity.Name, "1800-000-0911");
            }
            return Content(response);
        }

    }
}
