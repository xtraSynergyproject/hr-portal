using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AnalyzerViewModel: ViewModelBase
    {
        public long? AnalyzerUserId { get; set; }
        public string AnalyzerUserName { get; set; }
        public long? SharingUserId { get; set; }
        public string ReportLOVCode { get; set; }
        public string ReportLOVName { get; set; }
        [Required]
        [Display(Name = "Report Name")]
        public string ReportName { get; set; }
        public string Fields { get; set; }
        public string SearchCondition { get; set; }
        [Display(Name = "Shared with")]
        public string SharedWith { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string ReportDescription { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryLOVCode { get; set; }
        public string CategoryLOVName { get; set; }
        public bool IsOwner { get; set; }
        public string Query { get; set; }
        public string Udf { get; set; }
    }
}
