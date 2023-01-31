
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.WebUtility
{
    public class DataSourceRequestExtend: DataSourceRequest
    {
        public IList<IFilterDescriptor> Filter { get; set; }
    }
}
