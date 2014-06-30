using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class HomeViewModel
    {
        public ItemModel[] ItemModel { get; set; }
        public BasketViewModel BasketViewModel { get; set; }
    }
}