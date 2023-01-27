using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AssessmentViewModel : ServiceTemplateViewModel
    {
        public string AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentDescription { get; set; }
        public string AssessmentTypeId { get; set; }
        public string AssessmentType { get; set; }
        public string NoteId { get; set; }
        public string QuestionId { get; set; }
        public string JobId { get; set; }
        public int Weightage { get; set; }
        public long? AssessmentDuration { get; set; }
        public string JobTitle { get; set; }
        public long? TotalCount { get; set; }
        public double? Score { get; set; }
        public string AssessmentSetName { get; set; }
        public string AssessmentSetDescription { get; set; }
        public string Question { get; set; }

    }
}
