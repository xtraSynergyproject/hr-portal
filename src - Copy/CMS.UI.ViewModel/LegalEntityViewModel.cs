using CMS.Data.Model;
using CMS.Common;
using System;

namespace CMS.UI.ViewModel
{
    public class LegalEntityViewModel : LegalEntity
    {
        public string CountryName { get; set; }
        public int? FiscalYearStartMonth { get; set; }
        public int? FiscalYearEndMonth { get; set; }
        public FiscalYearTypeEnum? FiscalYearType { get; set; }
      
        //public double? BasicSalaryPercentage { get; set; }
       
       // public double? HousingAllowancePercentage { get; set; }
       
       // public double? TransportAllowancePercentage { get; set; }
    }
}
