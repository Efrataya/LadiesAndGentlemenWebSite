using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LadiesAndGentlemenWebSite.Models
{
    public class Client
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a first name")]
        [StringLength(300, ErrorMessage = "Hey, you can't enter more than 300 letters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a last name")]
        [StringLength(300, ErrorMessage = "Hey, you can't enter more than 300 letters!")]
        public string LastName { get; set; }

        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public Address Address { get; set; }

        [Required(ErrorMessage = "You must enter a phone number")]        
        [RegularExpression(@"^05[0,2, 3, 4,5, 8]{1}[-]{0,1}[\s\./0-9]{7}$")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       
        public DateTime DateOfBirth { get; set; }//איך מגבילים תאריך
       
        public ICollection<Order> Orders { get; set; }


    }
}