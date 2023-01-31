using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ERP.Utility
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string name) where T : struct
        {
            if (string.IsNullOrEmpty(name))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), name);
        }

        public static T? ToNullableEnum<T>(this string name) where T : struct
        {
            if (string.IsNullOrEmpty(name))
            {
                return default(T?);
            }
            return (T)Enum.Parse(typeof(T), name);
        }

        public static string Description(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return string.Empty;
            }
            DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
        public static IList<SelectListItem> SelectListFor(Type enumType, Enum value = null, params Enum[] param)
        {
            if (enumType.IsEnum)
            {
                var list = Enum.GetValues(enumType)
                    .Cast<Enum>()
                    .Where(i => !param.Contains(i))
                    .Select(e => new SelectListItem()
                    {
                        Value = Enum.GetName(enumType, e),
                        Text = e.Description(),
                        Selected = e.ToString() == Convert.ToString(value)
                    })
                    .ToList();
                return list;
            }

            return null;


            //if (enumType.IsEnum)
            //{
            //    var list = EnumHelper.GetSelectList(enumType, value);
            //    foreach (var item in param)
            //    {
            //        list = list.Where(x => x.Value != Convert.ToString(item)).ToList();
            //    }
            //    return list;
            //}

            //return null;
        }






    }
    public class EnumHiddenAttribute : Attribute
    {

    }
}
