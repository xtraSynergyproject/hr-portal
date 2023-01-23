using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class ServiceViewModel : NtsService
    {      
        public string PageName { get; set; }
        public string PageId { get; set; }
        public bool? OverrideUdfVisibility { get; set; }
        public string IsAdminMode { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerUserUserName { get; set; }
        public string OwnerMobile { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerGrade { get; set; }
        public DateTime? OwnerDateofJoin { get; set; }
        public long? ServiceStepTemplateId { get; set; }
        public long? StepTaskId { get; set; }
        public string StepTaskNo { get; set; }
        public long? ServiceStepServiceId { get; set; }
        public bool AllowPastStartDate { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string TemplateTemplateMasterName { get; set; }
        public string ServiceStatusCode { get; set; }
        public string ServiceStatusName { get; set; }
        public string Priority { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string ServiceTasks { get; set; }
        public string ServicePlusServices { get; set; }
        public bool CanAddStepTask { get; set; }
        public string StepTaskCreationOptionalLabel { get; set; }
        public bool IsConfidential { get; set; }
        public string Temporary { get; set; }
        public int ServiceTaskTemplateCount { get; set; }
        public int ServicePlusServiceTemplateCount { get; set; }
        public bool EnableAdhocTask { get; set; }
        public bool CanAddAdhocTask { get; set; }
        public string AdhocTaskHeaderMessage { get; set; }
      
        public string RemoveStepTaskSuccessMessage { get; set; }      

        public long? ServiceVersionId { get; set; }
        public long? FocusTaskId { get; set; }
        public string ServiceSharedList { get; set; }
        public string CancelButtonText { get; set; }
        public string PostComment { get; set; }

        public string CSVFileIds { get; set; }

        public bool? ChangeStatusOnStepChange { get; set; }
        public string ServiceOwnerText { get; set; }

        public bool? CreateInBackGround { get; set; }       
        public bool? IsCreatingInBackGround { get; set; }
        public double? PercentageCompleted { get; set; }
        public string XmlData { get; set; }

        public long? ReminderId { get; set; }
        public long? RecurrenceId { get; set; }
        public string Base64ImageOpCheckList { get; set; }
        public string ModulesList { get; set; }
       
        public string EventName { get; set; }
        public bool? IsSystemAutoService { get; set; }       

        public long? AttachmentCount { get; set; }
        public long? CommentCount { get; set; }
        public long? SharedCount { get; set; }
        public long? NotificationCount { get; set; }

        public long? ReferenceServiceId1 { get; set; }
        public long? ReferenceServiceId2 { get; set; }

        public long? ActualFocusId { get; set; }
        public long? PrevServiceId { get; set; }
        public long? NextServiceId { get; set; }
        public long? PrevTaskId { get; set; }
        public long? NextTaskId { get; set; }

        public long? RelatedNoteCount { get; set; }
        public string AssessmentUrl { get; set; }
        public long? DocumentId { get; set; }
        public IList<IdNameViewModel> ProjectList { get; set; }
        public IList<ServiceViewModel> SubStages { get; set; }
        public string StepTemplateIds { get; set; }
        public string DisplayDueDate { get; set; }
        public string ServiceDetailsHeight { get; set; }
        public string TemplateCategoryCode { get; set; }
        public TemplateCategoryTypeEnum? TemplateCategoryType { get; set; }
        public string RequestedByUserName { get; set; }
        public string CreatedByUserName { get; set; }
        public string LastUpdatedByUserName { get; set; }
        public string Module { get; set; }
        public NtsUserTypeEnum? TemplateUserType { get; set; } //set runtime
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string TemplateMasterCode { get; set; }
        public PerformanceDocumentStatusEnum? PerformanceDocumentStatus { get; set; }
        public string DueDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(DueDate);
                return d;
            }
        }
        
        public string CompletedDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(CompletedDate);
                return d;
            }
        }
        
        public string RejectedDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(RejectedDate);
                return d;
            }
        }
        
        public string CanceledDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(CanceledDate);
                return d;
            }
        } 
        
        public string ClosedDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(ClosedDate);
                return d;
            }
        }
        public string Weightage { get; set; }
        public string SuccessCriteria { get; set; }
        public string TemplateDisplayName { get; set; }
        public string StageName { get; set; }
        public string StageId { get; set; }
        public string DisplayStartDate { get; set; }
        public string NtsId { get; set; }
        public string MoveToParent { get; set; }
        public string MovePostionId { get; set; }
        public MovePostionEnum MovePostionSeq { get; set; }
        public BookMoveTypeEnum MoveType { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public bool Select { get; set; }
        public string PerformanceUser { get; set; }
        public string PerformanceStage { get; set; }
        public string PerformanceUserYear { get; set; }
        public string ManagerScore { get; set; }
        public string ManagerScoreValue { get; set; }
        public string ManagerComments { get; set; }
        public string DepartmentId { get; set; }
        public bool lazy { get; set; }
        public string title { get; set; }
        public string key { get; set; }
        public string Year { get; set; }
        public string MasterStageId { get; set; }

        public PerformanceDocumentStatusEnum DocumentStatus { get; set; }

        public string DocumentStatusName { get { return DocumentStatus.ToString(); } }
        
        public DateTime? EndDate { get; set; }    
        
        public string Location { get; set; }
        public string DepartmentName { get; set; }
        public string IconFileId { get; set; }
        public int? InprogressCount { get; set; }
        public int? CompletedCount { get; set; }
        public int? RejectedCount { get; set; }

    }
}
