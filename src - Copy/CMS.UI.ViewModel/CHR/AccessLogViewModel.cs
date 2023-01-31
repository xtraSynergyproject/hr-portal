using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.UI.ViewModel
{
    public class AccessLogViewModel : ViewModelBase
    {
        public string DeviceId { get; set; }
        public string BiometricId { get; set; }
   
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
        public string UserInfoId { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime[] PunchingTimeArray { get; set; }
        public string AccessLogText { get; set; }
        public string AccessLogSource { get; set; }
        public string PersonFullName { get; set; }
      
        public string SponsorshipNo { get; set; }
        //public int? Year { get; set; }
        //public int? Month { get; set; }  
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime StartDate { get; set; }
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime EndDate { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceId { get; set; }
        public PunchingTypeEnum SignInType { get; set; }
        public string AccessLogStatus { get; set; }
        public string ServiceOwner { get; set; }
        public string SignInLocation { get; set; }
      //  public long? GeoLocationId { get; set; }

    }
}