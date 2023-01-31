using Synergy.App.DataModel;
using Synergy.App.Common;
using System;

namespace Synergy.App.ViewModel
{
    public class CompanyViewModel : Company
    {
        public DataActionEnum DataAction { get; set; }
        public string CountryName { get; set; }
    }
}
