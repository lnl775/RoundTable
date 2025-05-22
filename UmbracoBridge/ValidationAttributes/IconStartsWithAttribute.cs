using System.ComponentModel.DataAnnotations;

namespace UmbracoBridge.ValidationAttributes
{
    public class IconStartsWithAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string icon && !icon.StartsWith("icon-"))
            {
                return new ValidationResult("Icon must start with 'icon-'.");
            }
            return ValidationResult.Success;
        }
    }
}
