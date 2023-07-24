using System.ComponentModel.DataAnnotations;

namespace letter_of_no_evidence.web.Helper
{
    public class RequiredIf : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue1 { get; set; }
        private object DesiredValue2 { get; set; }

        public RequiredIf(string propertyName, object desiredvalue1, object desiredvalue2 = null)
        {
            PropertyName = propertyName;
            DesiredValue1 = desiredvalue1;
            DesiredValue2 = desiredvalue2;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null) ?? string.Empty;
            if (DesiredValue2 == null &&
                (proprtyvalue.ToString() == DesiredValue1.ToString()) &&
                (value == null || string.IsNullOrEmpty(value.ToString())))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            if (DesiredValue2 != null &&
                (proprtyvalue.ToString() == DesiredValue1.ToString() || proprtyvalue.ToString() == DesiredValue2.ToString()) &&
                (value == null || string.IsNullOrEmpty(value.ToString())))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
