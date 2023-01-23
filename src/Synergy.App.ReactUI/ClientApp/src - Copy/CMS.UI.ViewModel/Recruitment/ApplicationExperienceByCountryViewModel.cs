using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class ApplicationExperienceByCountryViewModel : DataModelBase
    {
        public string CountryName { get; set; }
        public string ApplicationId { get; set; }
        public string CountryId { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
