using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PayrollPersonViewModel
    {
        public string PersonId { get; set; }
        public string AssignmentRootId { get; set; }
        public string AssignmentId { get; set; }
        public DateTime AssignmentStartDate { get; set; }
        public DateTime AssignmentEndDate { get; set; }
        public string UserId { get; set; }
        public string PersonTypeCode { get; set; }
        public string PersonTypeName { get; set; }
        public string PersonNo { get; set; }

        public DateTime? DateOfJoin { get; set; }
        public DateTime? LastWorkingDate { get; set; }

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
        public string NationalityId { get; set; }
        public string NationalityCode { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? WorkLocationId { get; set; }
        public string WorkLocationName { get; set; }
        public string SponsorshipNo { get; set; }


        public string ContractRootId { get; set; }
        public string ContractId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public ContractTypeEnum? ContractType { get; set; }
        public string SponsorId { get; set; }



        public string PayrollGroupId { get; set; }
        public string PayrollCalendarId { get; set; }


        public string SalaryInfoRootId { get; set; }
        public string SalaryInfoId { get; set; }
        public StatusEnum? SalaryInfoStatus { get; set; }
        public DateTime? SalaryInfoStartDate { get; set; }
        public DateTime? SalaryInfoEndDate { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankId { get; set; }
        public string BankBranchId { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }

        public PaymentModeEnum? PaymentMode { get; set; }
        public bool? TakeAttendanceFromTAA { get; set; }
        public bool? IsEligibleForOT { get; set; }
        public OTPaymentTypeEnum? OTPaymentType { get; set; }



        public bool IsPayrollActive { get; set; }
        public int PayrollNumberOfDays { get; set; }




        public string JobId { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
