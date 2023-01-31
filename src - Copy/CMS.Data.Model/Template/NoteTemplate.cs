using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NoteTemplate : DataModelBase
    {
        public bool EnableIndexPage { get; set; }
        public bool EnableNoteNumberManual { get; set; }
        public bool EnableSaveAsDraft { get; set; }
        public string SaveAsDraftText { get; set; }
        public string SaveAsDraftCss { get; set; }
        public string CompleteButtonText { get; set; }
        public string CompleteButtonCss { get; set; }
        public bool EnableBackButton { get; set; }
        public string BackButtonText { get; set; }
        public string BackButtonCss { get; set; }
        public bool EnableAttachment { get; set; }
        public bool EnableComment { get; set; }

        public bool DisableVersioning { get; set; }

        public string SaveNewVersionButtonText { get; set; }
        public string SaveNewVersionButtonCss { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("NoteIndexPageTemplate")]
        public string NoteIndexPageTemplateId { get; set; }
        public NoteIndexPageTemplate NoteIndexPageTemplate { get; set; }

        public CreateReturnTypeEnum CreateReturnType { get; set; }
        public EditReturnTypeEnum EditReturnType { get; set; }

        public string PreScript { get; set; }
        public string PostScript { get; set; }

        public string IconFileId { get; set; }
        public string TemplateColor { get; set; }
        public string BannerFileId { get; set; }
        public string BackgroundFileId { get; set; }
        public string OcrTemplateFileId { get; set; }

        public string Subject { get; set; }
        public string NotificationSubject { get; set; }
        public string Description { get; set; }

        public string SubjectText { get; set; }

        public string OwnerUserText { get; set; }
        public string RequestedByUserText { get; set; }
        [ForeignKey("Priority")]
        public string PriorityId { get; set; }
        public LOV Priority { get; set; }
        public bool EnableCancelButton { get; set; }
        public bool IsCancelReasonRequired { get; set; }
        //public bool IsCompleteReasonRequired { get; set; }        
        public string CancelButtonText { get; set; }
        public string CancelButtonCss { get; set; }
        public bool IsUdfTemplate { get; set; }
        public bool IsCompleteReasonRequired { get; set; }
        // public TimeSpan? SLA { get; set; }
        // public bool AllowSLAChange { get; set; }

        public string NoteNoText { get; set; }
        public string DescriptionText { get; set; }
        public bool HideHeader { get; set; }
        public bool HideSubject { get; set; }
        public bool HideDescription { get; set; }
        public bool HideStartDate { get; set; }
        public bool HideExpiryDate { get; set; }
        public bool HidePriority { get; set; }
        public bool HideOwner { get; set; }
        public bool IsSubjectMandatory { get; set; }
        public bool IsSubjectUnique { get; set; }
        public bool IsDescriptionMandatory { get; set; }
        public bool HideToolbar { get; set; }
        public bool HideBanner { get; set; }
        public bool AllowPastStartDate { get; set; }
        public bool EnablePrintButton { get; set; }
        public string PrintButtonText { get; set; }

        public bool EnableDataPermission { get; set; }
        [ForeignKey("ColumnMetadata")]
        public string DataPermissionColumnId { get; set; }
        public NumberGenerationTypeEnum NumberGenerationType { get; set; }
        public bool IsNumberNotMandatory { get; set; }
        public bool EnableLegalEntityFilter { get; set; }

        public bool EnablePermission { get; set; }
        public bool EnableInlineComment { get; set; }
        public string[] AdhocTaskTemplateIds { get; set; }
        public string[] AdhocServiceTemplateIds { get; set; }
        public string[] AdhocNoteTemplateIds { get; set; }
        public NoteTypeEnum? NoteTemplateType { get; set; }

        public string EmailCopyTemplateCode { get; set; }
        public FormTypeEnum FormType { get; set; }

        public bool EnableSequenceOrder { get; set; }
        public string SubjectMappingUdfId { get; set; }
        public string DescriptionMappingUdfId { get; set; }
        public ActionButtonPositionEnum ActionButtonPosition { get; set; }
        public string SubjectUdfMappingColumn { get; set; }
        public string DescriptionUdfMappingColumn { get; set; }
    }


    [Table("NoteTemplateLog", Schema = "log")]
    public class NoteTemplateLog : NoteTemplate
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }

}
