using System;
using System.ComponentModel.DataAnnotations;

namespace PriceScoutAPI.Helpers.Validations
{
    public class SortOrderValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var sortBy = value as string;

            if(string.IsNullOrEmpty(sortBy))
            {
                return ValidationResult.Success;
            }

            if(sortBy != null && (sortBy.ToUpper().Equals("DESC") || sortBy.ToUpper().Equals("ASC")))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("The value of 'sortBy' parameter must to be valid according to the API Documentation");
        }
    }
}
