using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserAssessmentScheduleSubmit
    {
        public  List<UserAssessmentScheduleViewModel> Created { get; set; }
        public  List<UserAssessmentScheduleViewModel> Updated { get; set; }
        public  List<UserAssessmentScheduleViewModel> Destroyed { get; set; }
    }

    public class UserReportSubmit
    {
        public List<UserReportSpreadsheetViewModel> Created { get; set; }
        public List<UserReportSpreadsheetViewModel> Updated { get; set; }
        public List<UserReportSpreadsheetViewModel> Destroyed { get; set; }
    }

    public class ApplicationAssessmentScheduleSubmit
    {
        public List<ApplicationAssessmentScheduleViewModel> Created { get; set; }
        public List<ApplicationAssessmentScheduleViewModel> Updated { get; set; }
        public List<ApplicationAssessmentScheduleViewModel> Destroyed { get; set; }
    }
    
    public class AssessmentResultSubmit
    {
        public List<UserAssessmentResultViewModel> Created { get; set; }
        public List<UserAssessmentResultViewModel> Updated { get; set; }
        public List<UserAssessmentResultViewModel> Destroyed { get; set; }
    }
}
