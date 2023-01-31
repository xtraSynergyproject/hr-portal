using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PayrollPersonViewModel : DatedViewModelBase
    {
        public long PersonId { get; set; }
        public long AssignmentRootId { get; set; }
        public long AssignmentId { get; set; }
        public DateTime AssignmentStartDate { get; set; }
        public DateTime AssignmentEndDate { get; set; }
        public long UserId { get; set; }
        public string PersonTypeCode { get; set; }
        public string PersonTypeName { get; set; }
        public string PersonNo { get; set; }

        public DateTime? DateOfJoin { get; set; }

        public PersonTitleEnum? Title { get; set; }


        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullNameLocal { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public GenderEnum? Gender { get; set; }
        public MaritalStatusEnum? MaritalStatus { get; set; }
        public ReligionEnum? Religion { get; set; }
        public long? NationalityId { get; set; }
        public string NationalityCode { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? WorkLocationId { get; set; }
        public string WorkLocationName { get; set; }
        public string SponsorshipNo { get; set; }


        public long? ContractRootId { get; set; }
        public long? ContractId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public ContractTypeEnum? ContractType { get; set; }
        public long? SponsorId { get; set; }


   
        public long? PayrollGroupId { get; set; }
        public long? PayrollCalendarId { get; set; }


        public long? SalaryInfoRootId { get; set; }
        public long? SalaryInfoId { get; set; }
        public StatusEnum? SalaryInfoStatus { get; set; }
        public DateTime? SalaryInfoStartDate{ get; set; }
        public DateTime? SalaryInfoEndDate { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }

        public PaymentModeEnum? PaymentMode { get; set; }
        public bool? TakeAttendanceFromTAA { get; set; }
        public bool? IsEligibleForOT { get; set; }
        public OTPaymentTypeEnum? OTPaymentType { get; set; }



        public bool IsPayrollActive { get; set; }
        public int PayrollNumberOfDays { get; set; }




        public long? JobId { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? LocationId { get; set; }
        public string LocationName { get; set; }


    }
}
