using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common.Enums;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
  public  class UserInfoViewModel
    {
        public string Id { get; set; }
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
        [Display(Name = "DeviceId")]
        public string DeviceId { get; set; }
        public long? DeviceRelationshipId { get; set; }
        public IncludeExclude? UserIncludeOrExclude { get; set; }

        [Display(Name = "Device")]
        public string DeviceName { get; set; }
        public string JobName { get; set; }
        public string PhotoName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string PersonId { get; set; }
        public string UserId { get; set; }
     //   public List<UserInfoImageViewModel> UserImages { get; set; }

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
                    ret = string.Concat(ret, device, ",");
                }
                return ret.Trim(',');

            }
        }

        public string[] RegisteredDeviceList { get; set; }

        
        public string userInfoId { get; set; }

        public IncludeExclude? IncludeOrExclude { get; set; }

        public string RegistrationStatus { get; set; }
        public string IsRegisteredDevice { get; set; }


    }
}
