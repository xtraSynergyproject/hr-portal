using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateEvaluation", Schema = "rec")]
    public class CandidateEvaluation : DataModelBase
    {
        public string EvaluationName { get; set; }
        public string EvaluationScale1 { get; set; }
        public bool IsEvaluationScale1 { get; set; }
        public string EvaluationScale2 { get; set; }
        public bool IsEvaluationScale2 { get; set; }
        public string EvaluationScale3 { get; set; }
        public bool IsEvaluationScale3 { get; set; }
        public string Marks { get; set; }
        public bool IsTemplate { get; set; }
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
