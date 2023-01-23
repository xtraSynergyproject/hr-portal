using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
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
        public string CasteName { get; set; }
        public string Category { get; set; }
      
        public string CategoryNumber { get; set; }
    }
}
