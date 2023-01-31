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
    public class DateCompareAttribute : ValidationAttribute
    {
        public DateCompareAttribute(string fromDate, bool allowEqual = true)
        {
            FromDate = fromDate;
            AllowEqual = allowEqual;
        }
        public virtual string FromDate { get; set; }
        public virtual bool AllowEqual { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            DateTime laterDate = (DateTime)validationContext.ObjectType.GetProperty(FromDate).GetValue(validationContext.ObjectInstance, null);
            var fromDateProp = validationContext.ObjectType.GetProperty(FromDate);
            var fromDateDisplayAttribute = fromDateProp.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            var fromDateDisplayName = "";
            
            if (fromDateDisplayAttribute != null)
            {
                fromDateDisplayName = fromDateDisplayAttribute.Name;
            }
            else
            {
                fromDateDisplayName = FromDate;
            }
            var fromDateValue = fromDateProp.GetValue(validationContext.ObjectInstance, null);
            if (fromDateValue == null)
            {
                return null;
            }
            var fromDate = Convert.ToDateTime(fromDateValue);
            var toDate = Convert.ToDateTime(value);
            if (AllowEqual)
            {
                if (fromDate > toDate)
                {
                    return new ValidationResult(String.Concat(validationContext.DisplayName, " must be greater than or equal to ", fromDateDisplayName));
                }
            }
            else
            {
                if (fromDate >= toDate)
                {
                    return new ValidationResult(String.Concat(validationContext.DisplayName, " must be greater than ", fromDateDisplayName));
                }
            }
            return null;
        }
    }
}
