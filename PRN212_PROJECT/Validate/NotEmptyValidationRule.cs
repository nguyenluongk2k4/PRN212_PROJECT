using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PRN212_PROJECT.Validate
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "This field cannot be empty.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
