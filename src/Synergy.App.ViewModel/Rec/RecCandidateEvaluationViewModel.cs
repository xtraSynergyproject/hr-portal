using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class RecCandidateEvaluationViewModel : NoteTemplateViewModel
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
        public long? SequenceOrder { get; set; }
        public string ApplicationId { get; set; }
        public string CandidateEvaluationNoteId { get; set; }

    }
}
