using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AssessmentQuestionSubmit
    {
        public  List<AssessmentQuestionOptionViewModel> Created { get; set; }
        public  List<AssessmentQuestionOptionViewModel> Updated { get; set; }
        public  List<AssessmentQuestionOptionViewModel> Destroyed { get; set; }
    }
}
