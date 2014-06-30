using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;
using OnlineStore.Common;
using System.Net.Http;
using System.Net;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        //
        // GET: /Store/

        private readonly IBasketManager basketManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreController"/> class.
        /// </summary>
        /// <param name="basketMgr">The basket MGR.</param>
        public StoreController(IBasketManager basketMgr)
        {
            basketManager = basketMgr;
        }


        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var items = new List<ItemModel>();
            HttpClient client = new HttpClient();
            var response = client.GetAsync(string.Format("http://localhost:87/service/api/MyCart")).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var masterProducts = response.Content.ReadAsAsync<OnlineStore.Contracts.Product[]>().Result;
                if (masterProducts != null)
                {
                    items = masterProducts.Select(p => new ItemModel
                    {
                        Id = p.ProductId,
                        ItemName = p.ProductName,
                        Image = string.Concat("../../Images/", p.ImagePath.Trim()),
                        Price = p.Price,
                        Unit = p.Unit
                    }).ToList();
                }
            }
            return View(items);
        }

        /// <summary>
        /// Views the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public ActionResult View(int Id)
        {
            var items = new List<ItemViewModel>();
            HttpClient client = new HttpClient();
            var response = client.GetAsync(string.Format("http://localhost:87/Service/api/MyCart")).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var masterProducts = response.Content.ReadAsAsync<OnlineStore.Contracts.Product[]>().Result;
                if (masterProducts != null)
                {
                    items = masterProducts.Select(p => new ItemViewModel
                    {
                        Id = p.ProductId,
                        Name = p.ProductName,
                        ImagePath = string.Concat("../../Images/", p.ImagePath.Trim()),
                        Price = p.Price,
                        Unit = p.Unit
                    }).ToList();
                }
            }

            var itemViewModel = items.FirstOrDefault(k => k.Id == Id);
            return View(itemViewModel ?? new ItemViewModel());
        }

        /// <summary>
        /// Searches the specified kind.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(string kind)
        {
            var items = new List<ItemModel>();
            HttpClient client = new HttpClient();
            var response = client.GetAsync(string.Format("http://localhost:87/Service/api/MyCart")).Result;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var masterProducts = response.Content.ReadAsAsync<OnlineStore.Contracts.Product[]>().Result;
                if (masterProducts != null)
                {
                    items = masterProducts.Select(p => new ItemModel
                    {
                        Id = p.ProductId,
                        ItemName = p.ProductName,
                        Image = string.Concat("../../Images/", p.ImagePath.Trim()),
                        Price = p.Price,
                        Unit = p.Unit
                    }).ToList();
                }
            }

            var results = items.Where(k => k.ItemName.Equals(kind, StringComparison.OrdinalIgnoreCase));

            return View("Index", results);
        }

    }
}
