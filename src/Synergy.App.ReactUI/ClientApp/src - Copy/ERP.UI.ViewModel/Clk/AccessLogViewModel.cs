using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class AccessLogViewModel : ViewModelBase
    {
        public long DeviceId { get; set; }
        public string BiometricId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime PunchingTime { get; set; }
        public string DeviceName { get; set; }
        public int DeviceMachineNo { get; set; }
        public string DeviceIpAddress { get; set; }
        public int DevicePortNo { get; set; }
        public string DeviceSerialNo { get; set; }
        public PunchingTypeEnum DevicePunchingType { get; set; }
        public string DevicePunchingTypeText
        {
            get { return DevicePunchingType.Description(); }
        }
        public long? UserInfoId { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime[] PunchingTimeArray { get; set; }
        public string AccessLogText { get; set; }
        [Display(Name = "PersonFullName", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        //[Display(Name = "Employee Name")]
        public string PersonFullName { get; set; }
        //[Display(Name = "Iqamah No")]
        [Display(Name ="Iqamah No")]
        public string SponsorshipNo { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //[Display(Name = "Start Date")]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //[Display(Name = "End Date")]        
        [Display(Name = "EndDate", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public DateTime EndDate { get; set; }

        public string ServiceNo { get; set; }

        public long? ServiceId { get; set; }

        public PunchingTypeEnum SignInType { get; set; }
        public string AccessLogStatus { get; set; }

        public string ServiceOwner { get; set; }
        public string SignInLocation { get; set; }
        public long? GeoLocationId { get; set; }

    }
}
