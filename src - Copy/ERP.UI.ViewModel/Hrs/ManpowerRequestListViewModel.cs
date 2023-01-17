using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System;

namespace ERP.UI.ViewModel
{
    public class ManpowerRequestListViewModel : BaseViewModel
    {

       
        [Display(Name = "Grade")]
        public int? GradeId { get; set; }

        [Display(Name = "Grade")]
        public string Grade { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedByEmployeeNo { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedByEmployeeName { get; set; }

        [Display(Name = "Request Status")]
        public string TransactionStatus { get; set; }

        [DataType(DataType.Date)]
        [DateRange]
        [Display(Name = "Requested Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = AnnotationDeclaration.DateFormat)]
        public DateTime? RequestedDate { get; set; }

     
        public int ParentPositionId { get; set; }

      
        public int HierarchyNameId { get; set; }

        public int EmployeeId { get; set; }

        public int? OrgHierarchyNameId { get; set; }
    }
}
