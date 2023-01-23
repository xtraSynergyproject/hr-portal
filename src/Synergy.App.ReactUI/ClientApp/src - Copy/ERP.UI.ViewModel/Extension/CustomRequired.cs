using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel;

namespace ERP.UI.ViewModel
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomRequired : ValidationAttribute
    {
        public CustomRequired(string requiredFieldName = "IsRequired")
        {
            RequiredFieldName = requiredFieldName;
        }
        public virtual string RequiredFieldName { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
              
                return null;
            }

            var isRequired = (bool)validationContext.ObjectType.GetProperty(RequiredFieldName).GetValue(validationContext.ObjectInstance, null);
            if (isRequired && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return new ValidationResult("test");
            }
            return null;
        }
    }
}
