using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PerformanceDocumentMasterStageViewModel : ViewModelBase
    {
        public long PerformanceDocumentMasterStageId { get; set; }       
        public long PerformanceDocumentMasterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Master Stage Status")]
        public PMSMasterStageStatusEnum? MasterStageStatus { get; set; }
        [Required]
        [Display(Name = "Sequence No")]
        public long? SequenceNo { get; set; }

    }
}
