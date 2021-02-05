using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = " نام غذا")]
        public string Name { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "نام گروه")]
        public int GroupId { get; set; }

        //ارتباط بین گروه و محصولات

        [ForeignKey("GroupId")]

        public virtual Group Group { get; set; }
        
    }
}