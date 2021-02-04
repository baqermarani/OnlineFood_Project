using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models
{
    public class shoppingcartViewModels
    {
        [Key]
        public int ID { get; set; }
        public Product Product { get; set; }
        [Display(Name ="تعداد")]
        public int quantity { get; set; }
    }
}