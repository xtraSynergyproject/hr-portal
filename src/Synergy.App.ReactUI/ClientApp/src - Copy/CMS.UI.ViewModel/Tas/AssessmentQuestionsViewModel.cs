using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class AssessmentQuestionsViewModel : NoteTemplateViewModel
    {
        public string Question { get; set; }
        public string QuestionId { get; set; }
        public string QuestionArabic { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionDescriptionArabic { get; set; }
        public string QuestionAttachmentId { get; set; }
        public string QuestionArabicAttachmentId { get; set; }
        public string Options { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public string TopicId { get; set; }
        public string CompentencyLevelId { get; set; }

        public string Topic { get; set; }
        public string CompetencyLevel { get; set; }
        public string AssessmentId { get; set; }
        public string IndicatorId { get; set; }
        public string IndicatorName { get; set; }
        public string AssessmentTypeId { get; set; }
        public string AssessmentType { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public bool Select { get; set; }
        public bool lazy { get; set; }
        public string title { get; set; }
        public string key { get; set; }
    }
}
