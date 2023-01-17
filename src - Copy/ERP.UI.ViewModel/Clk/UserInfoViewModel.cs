using System;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.UI.ViewModel
{
    public class UserInfoViewModel : ViewModelBase
    {
        public string BiometricId { get; set; }
        public string PersonNo { get; set; }
        [Display(Name = "Iqamah No")]
        public string SponsorshipNo { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PersonFullName { get; set; }
 
        public int Privelage { get; set; }
        public bool Enabled { get; set; }
        public string Password { get; set; }
       
        //[Display(Name="Device")]
        [Display(Name = "DeviceId", ResourceType = typeof(ERP.Translation.Clk.Clk))]
        public long? DeviceId { get; set; }
        public long? DeviceRelationshipId { get; set; }
        public IncludeExclude? UserIncludeOrExclude { get; set; }

        [Display(Name = "Device")]
        public string DeviceName { get; set; }
        public string JobName { get; set; }
        public string PhotoName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public long? PersonId { get; set; }
        public long? UserId { get; set; }
        public List<UserInfoImageViewModel> UserImages { get; set; }

        public string RegisteredDevices
        {
            get
            {
                if (RegisteredDeviceList == null || RegisteredDeviceList.Length == 0)
                {
                    return string.Empty;
                }
                var ret = string.Empty;
                foreach (var item in RegisteredDeviceList)
                {
                    var split = item.Split('|');
                    var device = split[0];
                    //var date = split[1].ToSafeDateTime();
                    //var status = split[2];
                    //if (status == ProcessStatusEnum.Completed.ToString())
                    //{
                    //    status = "Done";
                    //}
                    //else
                    //{
                    //    status = "In Progress";
                    //}
                    ret = string.Concat(ret, device,  ",");
                }
                return ret.Trim(',');

            }
        }

        public string[] RegisteredDeviceList { get; set; }

    }
}
