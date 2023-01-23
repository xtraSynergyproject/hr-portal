using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ContractViewModel : DatedViewModelBase
    {
        public long ContractId { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypeEnum? ContractType { get; set; }

        [Display(Name = "Contract Reference No")]
        public string ContractReferenceNo { get; set; }
        [Required]
        [Display(Name = "Employee")]
        public long PersonId { get; set; }
        [Display(Name = "Employee Name")]
        public string PersonName { get; set; }
        [Display(Name = "Annual Leave Entitlement")]
        public double? AnnualLeaveEntitlement { get; set; }
        public double? Basic { get; set; }
        public double? HRA { get; set; }
        public double? Transport { get; set; }
        [Display(Name = "Food Allowance")]
        public double? FoodAllowance { get; set; }
        public double? Others { get; set; }
        [Display(Name = "Sponsor")]
        public long? SponsorId { get; set; }
        [Display(Name = "Sponsor")]
        public string SponsorName { get; set; }
        [Display(Name = "Contract Renewable")]
        public BoolStatus? ContractRenewable { get; set; }
        public long? PositionId { get; set; }

        public long? ContractRootId { get; set; }


        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public int AirTicketInterval { get; set; }

    }
}
