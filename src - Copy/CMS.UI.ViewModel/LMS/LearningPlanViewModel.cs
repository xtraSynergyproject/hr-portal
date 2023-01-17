using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class LearningPlanViewModel: NoteTemplateViewModel
    {
        public string CourseId { get; set; }
        public DateTime LearningStartDate { get; set; }
        public DateTime LearningEndDate { get; set; }
        public string JobId { get; set; }
        public string CompetencyId { get; set; }
        public string CompetencyName { get; set; }
        public string CourseName { get; set; }
        public long Completed { get; set; }
        public string JsonData { get; set; }
    }
}
