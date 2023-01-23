using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class PayPropertyTaxViewModel
    {
        public string Id { get; set; }
        public string PropertyId { get; set; }
        public string OwnerName { get; set; }
        public string WardNo { get; set; }
        public string Locality { get; set; }
        public string Colony { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Zone { get; set; }
        public string OldArrear { get; set; }
        public string CurrentYearTax { get; set; }
        public string TotalRebateGiven  { get; set; }
        public double Total { get; set; }
        public string OldPropertyId { get; set; }
        public string HouseNo { get; set; }
        public string PostalCode { get; set; }
        public string CityId { get; set; }
        public string WardNoId { get; set; }
        public string CityName { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string RateZone { get; set; }


        public double PropertyTax { get; set; }
        public double EducationCess { get; set; }
        public double UrbanDev { get; set; }
        public double SamekitKar { get; set; }
        public double SewaKar { get; set; }
        public double AdditionalSamekitKar { get; set; }
        public double Rebate { get; set; }
        public double Penalty { get; set; }
        public double AnnualLettingvalue { get; set; }
        public double TotalABC { get; set; }
        public string Year { get; set; }
        public double NetALV { get; set; }

        public string Otp { get; set; }
        public string UlbId { get; set; }
        public string PropertyServiceId { get; set; }
        public DateTime OtpExpiryDate { get; set; }
        public string NDCAttachmentId { get; set; }
    }
}
