using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Statch.Models
{
    public class Users
    {
        public int UserID { get; set; }

        //UserName
        [Required(ErrorMessage = "User Name Required!")]
        [Display(Name = "User Name")]
        [StringLength(50, ErrorMessage = "User Name cannot be more than 50 characters!")]
        public string UserName { get; set; }

        //Salutation
        [StringLength(5)]
        public string Salutation { get; set; } //Nullable - from db


        //EmailAddr
        [Required(ErrorMessage = "Email Address Required!")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [ValidateUserExists]
        [StringLength(50, ErrorMessage = "Email length cannot be more than 50 characters!")]
        public string EmailAddr { get; set; }

        //Password
        //Default value set
        [Required(ErrorMessage = "Password Required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password must be at least 5 characters long!", MinimumLength = 5)]
        public string Password { get; set; }

        //DateCreated
        public DateTime DateCreated { get; set; }

    }
}
