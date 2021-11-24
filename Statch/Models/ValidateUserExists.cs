using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Statch.DAL;

namespace Statch.Models
{
    public class ValidateUserExists : ValidationAttribute
    {
        private UsersDAL userContext = new UsersDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Judge" model class
            Users user = (Users)validationContext.ObjectInstance;
            // Get the JudgeID from the Judge instance
            int userId = user.UserID;

            if (userContext.IsUserExist(email, userId))
            {
                // validation failed
                return new ValidationResult("Email address already exists!");
            }
            else
            {
                // validation passed
                return ValidationResult.Success;
            }
        }
    }
}
