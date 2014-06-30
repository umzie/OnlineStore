using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class PayLaterController : Controller
    {
        //
        // GET: /PayLater/

        public ActionResult Index(string transactionId)
        {
            return View();
        }

       

    }
}
