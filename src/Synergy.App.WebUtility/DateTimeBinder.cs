using Synergy.App.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

public class DateTimeBinder
    : IModelBinder
{
    /// <summary>  
    /// The user culture  
    /// </summary>  
    protected readonly IUserContext _userContext;
    /// <summary>  
    /// Initializes a new instance of the <see cref="DateTimeBinder"/> class.  
    /// </summary>  
    /// <param name="userCulture">The user culture.</param>  
    public DateTimeBinder(IUserContext userContext)
    {
        _userContext = userContext;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }
        var valueProviderResult = bindingContext.ValueProvider
          .GetValue(bindingContext.ModelName);
        if (string.IsNullOrEmpty(valueProviderResult.FirstValue))
        {
            return Task.CompletedTask;
        }
        DateTime datetime;
        if (DateTime.TryParse(valueProviderResult.FirstValue, out datetime))
        {
            datetime = _userContext.GetServerTime(datetime);
            bindingContext.Result = ModelBindingResult.Success(datetime);
        }
        else
        {
            // TODO: [Enhancement] Could be implemented in better way.  
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                bindingContext.ModelMetadata
                .ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor(
                  valueProviderResult.ToString(), nameof(DateTime)));
        }
        return Task.CompletedTask;
    }
}