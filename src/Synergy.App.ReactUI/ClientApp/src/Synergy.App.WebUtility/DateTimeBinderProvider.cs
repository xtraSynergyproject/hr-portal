using Synergy.App.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
/// <summary>  
/// <see cref="DateTimeBinder"/> provider.  
/// </summary>  
/// <seealso cref="IModelBinderProvider" />  
public class DateTimeBinderProvider
    : IModelBinderProvider
{
    private readonly IUserContext _uc;
    public DateTimeBinderProvider(IServiceCollection serviceCollection)
    {
        _uc = (IUserContext)serviceCollection.BuildServiceProvider().GetService(typeof(IUserContext));

    }
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        if (context.Metadata.UnderlyingOrModelType == typeof(DateTime))
        {
            return new DateTimeBinder(_uc);
        }
        return null; // TODO: Find alternate.  
    }
}