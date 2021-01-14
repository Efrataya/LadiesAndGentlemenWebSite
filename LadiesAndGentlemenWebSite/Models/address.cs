using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LadiesAndGentlemenWebSite.Models
{
    public class Address
    {
       //test
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a street name")]
        [StringLength(300, ErrorMessage = "Hey, you can't enter more than 300 letters!")]
        public string Street { get; set; }

        [Required(ErrorMessage = "You must enter a city name")]
        [StringLength(300, ErrorMessage = "Hey, you can't enter more than 300 letters!")]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter a housenumber")]
        [Range(0, 50000, ErrorMessage = "Hey, you must enter a number between 0 to 50000!")]
        public int HouseNumber { get; set; }

        [Range(0, 999999, ErrorMessage = "Hey, you must enter a number between 0 to 999999!")]
        public int Zip { get; set; }
    }
}