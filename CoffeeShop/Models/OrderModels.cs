using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeShop.Models
{
    public class Order
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Phonenumber { get; set; }

        public string Address { get; set; }

        public int Totalprice { get; set; }

        public bool Ispayed { get; set; }

        public bool Isdelivered { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }


    }
}