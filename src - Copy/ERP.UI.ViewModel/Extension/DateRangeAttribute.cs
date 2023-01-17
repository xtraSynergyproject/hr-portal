using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateRangeAttribute : ValidationAttribute
    {
        public DateRangeAttribute()
        {
            //if (minDate != null)
            //{
            //    MinDate = minDate.Value;
            //}
            //else
            //{
            //    MinDate = Constant.Annotation.MinimumDate;
            //}
            //if (minDate != null)
            //{
            //    MinDate = minDate.Value;
            //}
            //else
            //{
            //    MinDate = Constant.Annotation.MinimumDate;
            //}
            MinDate = Constant.ApplicationMinDate ;
            MaxDate = Constant.ApplicationMaxDate;
        }
        public virtual DateTime MinDate { get; set; }
        public virtual DateTime MaxDate { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            var d = (DateTime)value;
            if (d < MinDate || d > MaxDate)
            {
                return new ValidationResult(String.Concat("Value for ", validationContext.DisplayName, " must be between ",
                    string.Format(Constant.Annotation.DefaultDateFormat, Constant.ApplicationMinDate), " and ",
                    string.Format(Constant.Annotation.DefaultDateFormat, Constant.ApplicationMaxDate)));
            }

            return null;
        }
    }
}
