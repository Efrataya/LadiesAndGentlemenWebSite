using LadiesAndGentlemenWebSite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LadiesAndGentlemenWebSite.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter an image link")]
        [StringLength(300, ErrorMessage = "Hey, you can't enter more than 300 letters!")]
        //[RegularExpression(@"(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")]
        public string Image { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "You must enter a description")]
        [StringLength(999, ErrorMessage = "Hey, you can't enter more than 999 letters!")]
        public string Description { get; set; }
        public float price { get; set; }
        public ICollection<Cart> Carts { get; set; }

    }
}