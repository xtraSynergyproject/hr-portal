using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class AssessmentQuestionViewModel : ViewModelBase
    {
        public string Subject { get; set; }
        public string Description { get; set; }   
        public long? SerialNo { get; set; }
        public string Option { get; set; }
        public bool IsAnswer { get; set; }
        public long SequenceNo { get; set; }
        public AssessmentTypeEnum? AssessmentType { get; set; }
        public string JobName { get; set; }
        public string AnswerComment { get; set; }
        public Language PreferredLanguage { get; set; }
        public string Data { get; set; }
    }
    
}
