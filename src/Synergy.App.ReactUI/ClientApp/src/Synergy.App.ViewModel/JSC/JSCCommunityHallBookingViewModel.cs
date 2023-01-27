using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCCommunityHallBookingViewModel:ServiceTemplateViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PANNo { get; set; }
        public string Aadhar { get; set; }

        public string IsJmcEmployeeId { get; set; }
        public double JMCDiscountPercentage { get; set; }
        public string BookingById { get; set; }
        public string PaymentMode { get; set; }
        public string DateSelectionType { get; set; }

        public DateTime? BookingFromDate { get; set; }

        public DateTime? BookingToDate { get; set; }
        public string MultipleDates { get; set; }
        public double GrandTotal { get; set; }
        public double Amount { get; set; }
        public string JSC_CommunityHallBooking { get; set; }
        public List<CommunityHallBooking> CommunityHallBookingList { get; set; }
        public string PaymentStatusId { get; set; }
        public string PaymentReferenceNo { get; set; }
        public string ParcelId { get; set; }
        public string RevenueTypeId { get; set; }
        public string FunctionTypeId { get; set; }
        

    }
    public class CommunityHallBooking
    {
        public string CommunityHallId { get; set; }
        public DateTime CommunityBookingFromDate { get; set; }
        public DateTime CommunityBookingToDate { get; set; }
        public long NoOfDays { get; set; }

        public double Rate { get; set; }
        public double TotalAmount { get; set; }
        public string ParentId { get; set; }

        public string Id { get; set; }
        
    }
}
