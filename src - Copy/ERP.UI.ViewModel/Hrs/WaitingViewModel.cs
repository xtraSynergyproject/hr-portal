using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class WaitingViewModel : ViewModelBase
    {
        public string Name { get; set; }
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }
        public string Persons { get; set; }
        public string TableNumber { get; set; }
        public string Section { get; set; }
        public string MobileNo { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public string RestaurantIds { get; set; }
        public string UserName { get; set; }
        public long OwnerUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string WaitingTime { get; set; }

      //  public int WaitingTime { get {
      //          if (CreatedDate != null && UpdatedDate != null)
      //          {
      //              return (int)(UpdatedDate - CreatedDate).TotalMinutes;
      //          }
      //          else
      //          {
      //              return 0;
      //          }
      //      } }

    }
}
