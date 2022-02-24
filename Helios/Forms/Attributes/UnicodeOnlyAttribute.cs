using System.ComponentModel.DataAnnotations;

namespace Helios.Attributes;

public class UnicodeOnlyAttribute : ValidationAttribute {

    public UnicodeOnlyAttribute() { }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        if ( value is not string str ) return ValidationResult.Success;

        return str.Any(character => character >= 255)
            ? new ValidationResult($"The string provided does not conform to unicode standards.")
            : ValidationResult.Success;
    }
}