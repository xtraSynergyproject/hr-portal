using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class AssessmentReportViewModel:ViewModelBase
    {

        public string OwnerUserName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string CaseStudyText { get; set; }
        public string FileId { get; set; }
        public List<AssessmentDetailViewModel> TechnicalAssessment { get; set; }
        public AssessmentDetailViewModel CaseStudyAssessment { get; set; }
    }
}
