using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models
{
    public class Group
    {


        public int ID { get; set; }

        [Display(Name = "نام گروه")]
        public string Name { get; set; }

        //ارتباط بین گروه ومحصولات

        public virtual ICollection<Product> Products { get; set; }
    }
}