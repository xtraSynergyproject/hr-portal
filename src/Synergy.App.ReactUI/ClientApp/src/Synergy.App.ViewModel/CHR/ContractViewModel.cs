using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class ContractViewModel:ViewModelBase
    {
        public string ContractId { get; set; }
        public string NtsNoteId { get; set; }


        [Display(Name = "Contract Type")]
        public ContractTypeEnum? ContractType { get; set; }

        [Display(Name = "Contract Reference No")]
        public string ContractReferenceNo { get; set; }
       
        [Display(Name = "Employee")]
        public string PersonId { get; set; }
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
        public string SponsorId { get; set; }
        [Display(Name = "Sponsor")]
        public string SponsorName { get; set; }
        [Display(Name = "Contract Renewable")]
        public string ContractRenewable { get; set; }
        public string PositionId { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }




        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }
        public DateTime? DateOfJoin { get; set; }
        public int AirTicketInterval { get; set; }

    }
}
