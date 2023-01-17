using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class RestaurantWaitingViewModel: ViewModelBase
    {

     //   public long Id { get; set; }
        public long LocalId { get; set; }
        public string BookingDate { get; set; }
        public string Name { get; set; }
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }
        public string MobileNo { get; set; }
        public string BookingTime { get; set; }
        public string Section { get; set; }
        public string Persons { get; set; }
        public string WaitingStatus { get; set; }
        public string TableNumber { get; set; }
        public string WaitingTime { get; set; }
        public DateTime CreatedTime { get; set; }
     //   public long CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long UpdatedBy { get; set; }
        public long UserId { get; set; }

        public string Exceptions { get; set; }

    }
}
