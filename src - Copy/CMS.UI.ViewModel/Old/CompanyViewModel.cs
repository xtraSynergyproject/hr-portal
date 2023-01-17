using CMS.Data.Model;
using CMS.Common;
using System;

namespace CMS.UI.ViewModel
{
    public class CompanyViewModel : Company
    {
        public DataActionEnum DataAction { get; set; }
        public string CountryName { get; set; }
    }
}
