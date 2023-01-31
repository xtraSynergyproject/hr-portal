using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TicketAccrualViewModel
    {
        public long PersonId { get; set; }
        public string PersonNo { get; set; }
        
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public long UnpaidLeaveDays { get; set; }
        // public long AssignmentMasterId { get; set; }        

        public double AverageEconomyTicketCost { get; set; }

        public double AverageBusinessTicketCost { get; set; }
        public TravelClassEnum TravelClass { get; set; }
        public bool IsEligibleForAirTicketForSelf { get; set; }
        public bool IsEligibleForAirTicketForDependant { get; set; }


        public double TotalSalary { get; set; }
        public double BasicSalary { get; set; }
        public int AdultCount { get; set; }
        public int KidsCount { get; set; }
        public int InfantCount { get; set; }
        public int HusbandCount { get; set; }
        public int WifeCount { get; set; }
        public double UnpaidLeavesNotInSystem { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public double MonthlyAccrualDays { get; set; }
        public double AnnualLeaveEntitlement { get; set; }
        public bool IsEligibleForTicketClaim { get; set; }

    }
}