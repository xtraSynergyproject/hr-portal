﻿using CMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
/// <summary>  
/// JSON Date time converter.  
/// </summary>  
/// <seealso cref="DateTimeConverterBase" />  
public class DateTimeConverter
: DateTimeConverterBase
{
    /// <summary>  
    /// The user culture  
    /// </summary>  
    protected readonly Func<IUserContext> _userContext;
    /// <summary>  
    /// Initializes a new instance of the <see cref="DateTimeConverter"/> class.  
    /// </summary>  
    /// <param name="userCulture">The user culture.</param>  
    public DateTimeConverter(Func<IUserContext> userContext)
    {
        _userContext = userContext;
    }
    /// <summary>  
    /// Determines whether this instance can convert the specified object type.  
    /// </summary>  
    /// <param name="objectType">Type of the object.</param>  
    /// <returns>  
    /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.  
    /// </returns>  
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
    /// <summary>  
    /// Reads the JSON representation of the object.  
    /// </summary>  
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>  
    /// <param name="objectType">Type of the object.</param>  
    /// <param name="existingValue">The existing value of object being read.</param>  
    /// <param name="serializer">The calling serializer.</param>  
    /// <returns>  
    /// The object value.  
    /// </returns>  
    /// <exception cref="System.NotImplementedException"></exception>  
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
    /// <summary>  
    /// Writes the JSON representation of the object.  
    /// </summary>  
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>  
    /// <param name="value">The value.</param>  
    /// <param name="serializer">The calling serializer.</param>  
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Null are already filtered  
        var userCulture = _userContext();
        writer.WriteValue(TimeZoneInfo.ConvertTime(Convert.ToDateTime(value), userCulture.TimeZone)
        .ToString(userCulture.DateTimeFormat));
        writer.Flush();
    }
}