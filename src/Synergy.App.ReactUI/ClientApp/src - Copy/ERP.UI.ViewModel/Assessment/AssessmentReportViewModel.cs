using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class AssessmentReportViewModel : ViewModelBase
    {       

        public string OwnerUserName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string CaseStudyText { get; set; }
        public long? FileId { get; set; }
        public List<AssessmentViewModel> TechnicalAssessment { get; set; }
        public AssessmentViewModel CaseStudyAssessment { get; set; }
    }
    
}
