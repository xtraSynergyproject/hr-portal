using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PerformanceDocumentStageViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }       
        public string Description { get; set; }       
        public int? Year { get; set; }
        public DateTime? StartDate { get; set; }       
        public DateTime? EndDate { get; set; }       
        public PerformanceDocumentStatusEnum? DocumentStatus { get; set; }
        public PerformanceDocumentStatusEnum? DocumentStageStatus { get; set; }
        public PerformanceDocumentStatusEnum? MasterDocumentStageStatus { get; set; }
        public PerformanceObjectiveStageEnum? PerformanceStageObjective { get; set; }
        public string ManagerGoalStageTemplateId { get; set; }
        public string EmployeeGoalStageTemplateId { get; set; }

        public string ManagerCompetencyStageTemplateId { get; set; }
        public string EmployeeCompetencyStageTemplateId { get; set; }
        public string DocumentMasterStageId { get; set; }
        public bool EnableReview { get; set; }
        public bool EnableIncrementLetter { get; set; }
        
        public DateTime? ReviewStartDate { get; set; }
        public string LetterTemplate { get; set; }
        public string EmployeeReviewTemplate { get; set; }
        public string ManagerReviewTemplate { get; set; }
        public string DocumentMasterId { get; set; }
        public string DocumentId { get; set; }
        public string ServiceId { get; set; }
        public string FinalRatingSource { get; set; }
        public string MasterName { get; set; }
    }
}
