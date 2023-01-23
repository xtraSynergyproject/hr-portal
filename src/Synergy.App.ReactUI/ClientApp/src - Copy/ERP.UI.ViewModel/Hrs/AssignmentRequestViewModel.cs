using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class AssignmentRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public string Errors { get; set; }

        public string Mode { get; set; }

        public int? AssignmentTableId { get; set; }

        public int? AssignmentId { get; set; }

        public int? GRPId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [Display(Name = "Employee No")]
        public string AssignmentEmployeeNo { get; set; }

        public int? EmployeeId { get; set; }

        public int? EmployeeRequestId { get; set; }

        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Required]
        [Display(Name = "Organization Name")]
        public int? OrganizationId { get; set; }

        [Display(Name = "Job Name")]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public int? JobId { get; set; }

        //[Display(Name = "MyDAS Job Name")]
        //public string MyDASJobName { get; set; }

        //[Required]
        //[Display(Name = "MyDAS Job Name")]
        //public int? MyDASJobId { get; set; }

        [Display(Name = "Grade Name")]
        public string GradeName { get; set; }

        [Required]
        [Display(Name = "Grade Name")]
        public int? GradeId { get; set; }

        [Display(Name = "Position Name")]
        public string PositionName { get; set; }


        [Required]
        [Display(Name = "Position Name")]
        public int? PositionId { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Required]
        [Display(Name = "Location Name")]
        public int? LocationId { get; set; }

        [Display(Name = "Supervisor Name")]
        public string SupervisorName { get; set; }

        public int? SupervisorId { get; set; }

        [Display(Name = "Assignment Type Name")]
        public string AssignmentTypeName { get; set; }

        public int NextTab { get; set; }

        


        [Display(Name = "Assignment Type Name")]
        public int? AssignmentTypeId { get; set; }

        [Display(Name = "Assignment Status Name")]
        public string AssignmentStatusName { get; set; }

        [Display(Name = "Assignment Status Name")]
        public int? AssignmentStatusId { get; set; }

        [DataType(DataType.Date)]
        [DateRange]
        [Display(Name = "Actual Termination Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? ActualTerminationDate { get; set; }

        [Display(Name = "Assignment Remarks")]
        public string Remarks { get; set; }

        [DataType(DataType.Date)]
        [DateRange]
        [Display(Name = "Probation End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? ProbationEndDate { get; set; }


        [Display(Name = "Probation Days")]
        [Required]
        public int? ProbationDays { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of Join")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? DateOfJoin { get; set; }

        [Display(Name = "Email")]
       // [Required]
        public string CorporateEmail { get; set; }

        [Display(Name = "People Group")]
        [Required]
        public string PeopleGroup { get; set; }

        [Display(Name = "Ticket Destination")]
        public string TicketDestination { get; set; }

        [Display(Name = "Ticket Destination")]
        public string TicketDestinationId { get; set; }

        [Required]
        [Display(Name = "Pay Annual Ticket")]
        public string PayAnnualTicket { get; set; }

        [Required]
        [Display(Name = "Class Of Travel")]
        public string ClassOfTravel { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DateRange]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        [Display(Name = "Assignment Effective From Date")]
        public  DateTime? AssignmentEffectiveFromDate { get; set; }


        [DataType(DataType.Date)]
        [DateRange]
        [DateCompare("AssignmentEffectiveFromDate")]
        [Display(Name = "Assignment Effective To Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? AssignmentEffectiveToDate { get; set; }




    }
}
