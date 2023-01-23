using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ERP.UI.ViewModel
{
    public class InspectionViewModel : ViewModelBase
    {
        [Display(Name = "Service No.")]
        public string ServiceNo { get; set; }
        [Display(Name = "Inspector")]
        public long InspectorId { get; set; }
        [Display(Name = "Inspector")]
        public string InspectorName { get; set; }
        [Required]
        [Display(Name = "Inspection Type")]
        public InspectionTypeEnum? InspectionType { get; set; }        
        [Display(Name = "Inspection Template")]
        public long InspectionTemplateId { get; set; }
        [Display(Name = "Inspection Template")]
        public string InspectionTemplateName { get; set; }
        [Required]
        [Display(Name = "Inspection Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime InspectionDate { get; set; }
        //[Display(Name = "Inspection Time")]
        //public DateTime InspectionTime { get; set; }
        [Required]
        [Display(Name = "Project")]
        public long ProjectId { get; set; }
        [Display(Name = "Project")]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public long UnitId { get; set; }
        [Display(Name = "Unit")]
        public string UnitName { get; set; }
        public NtsActionEnum? InspectionStatus { get; set; }
        public string Quotation { get; set; }
        public string Amount { get; set; }
    }
}
