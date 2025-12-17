using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Entity.Validations
{
    public class LionNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Lion Name is required.");
            }

            string lionName = value.ToString()!;

            // Check minimum 4 characters
            if (lionName.Length < 4)
            {
                return new ValidationResult("Lion Name must be at least 4 characters long.");
            }

            // Check for special characters (#, @, &, (, ))
            if (Regex.IsMatch(lionName, @"[#@&()]"))
            {
                return new ValidationResult("Lion Name cannot contain special characters (#, @, &, (, )).");
            }

            // Check if each word starts with a capital letter
            string[] words = lionName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (!char.IsUpper(word[0]))
                {
                    return new ValidationResult("Each word in Lion Name must start with a capital letter.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
