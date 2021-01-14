using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LadiesAndGentlemenWebSite.Models
{
    public class Order
    {

        public int Id { get; set; }
        public Client Client { get; set; }
        public float Sum { get; set; }
        public ICollection<Cart> Carts { get; set; }


    }
}