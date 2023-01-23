using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class WorkStructureStatusViewModel 
    {
        public long PersonId { get; set; }
        public long? Id { get; set; }
        public string FullName { get; set; }
        public BoolStatus IsEmployeeActive { get; set; }
        public BoolStatus HasNationality { get; set; }
        public BoolStatus HasUser { get; set; }
        public BoolStatus HasAssignment { get; set; }
        public BoolStatus IsAssignmentActive { get; set; }
        public BoolStatus HasDateOfJoin { get; set; }
        public BoolStatus IsOrganizationActive { get; set; }
        public BoolStatus IsOrganizationInHierarchy { get; set; }
        public BoolStatus IsJobActive { get; set; }
        public BoolStatus IsPositionActive { get; set; }
        public BoolStatus IsPositionInHierarchy{ get; set; }
        public BoolStatus IsGradeActive { get; set; }

        public BoolStatus HasContract { get; set; }
        public BoolStatus IsContractActive { get; set; }
        public BoolStatus HasSponsor { get; set; }
        public BoolStatus HasSalaryInfo { get; set; }
        [Display(Name = "Has Payroll Department")]
        public BoolStatus HasPayrollOrganization { get; set; }
        public BoolStatus IsPayrollOrganizationActive { get; set; }
        public BoolStatus IsSalaryInfoActive { get; set; }
        [Display(Name = "Is PAyroll Department")]
        public BoolStatus IsPayrollOrganization { get; set; }

        public string UserName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DateOfJoin { get; set; }
        public string NationalityName { get; set; }
        public long? AssignmentId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        public string ParentOrganizationName { get; set; }
        public string JobName { get; set; }
        public string PositionName { get; set; }
        public string ParentPositionName { get; set; }
        public string GradeName { get; set; }
        public long? ContractId { get; set; }
        public string SponsorName { get; set; }
        public long? SalaryInfoId { get; set; }
        public long? InActiveContractId { get; set; }
        public DateTime? ContractEndDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? Date { get; set; }
        [Display(Name = "Expired Department")]
        public string ExpiredOrganization { get; set; }
        public string ExpiredPosition { get; set; }
        public string ExpiredJob { get; set; }
        public string PayrollOrganizationName { get; set; }
        public string PersonNo { get; set; }
        public string SponsorshipNo { get; set; }


    }
}
