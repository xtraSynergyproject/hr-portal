using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class SuccessionPlanningAssessmentViewModel
    {
        public string AssessmentId { get; set; }
        public string? AssessmentName { get; set; }
        public string AssessmentSetName { get; set; }
        public string AssessmentDescription { get; set; }
        public string AssessmentSetDescription { get; set; }
        public string AssessmentTypeId { get; set; }
        public string AssessmentType { get; set; }
        public string NoteId { get; set; }
        public string QuestionId { get; set; }
        public string JobId { get; set; }
        public int Weightage { get; set; }
        public TimeSpan? AssessmentDuration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
