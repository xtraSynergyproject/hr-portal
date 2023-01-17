using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PerformanceDocumentStageViewModel : ViewModelBase
    {
        //public long PerformanceDocumentStageId { get; set; }
        public long PerformanceDocumentMasterStageId { get; set; }
        public string PerformanceDocumentMasterName { get; set; }
        public long PerformanceDocumentId { get; set; }
        public long? PerformanceDocumentStageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletedDate { get; set; }
        public string Comment { get; set; }
        public PMSStageStatusEnum? StageStatus { get; set; }
        public string StartDateDisplay { get { return StartDate.ToDD_MMM_YYYY(); } }
        public string EndDateDisplay { get { return EndDate.ToDD_MMM_YYYY(); } }
        public string CompletedDateDisplay { get { return CompletedDate != null ? CompletedDate.ToDD_MMM_YYYY() : ""; } }
        public string StageStatusDisplay { get { return StageStatus.ToString(); } }
        public long? SequenceNo { get; set; }
    }
}
