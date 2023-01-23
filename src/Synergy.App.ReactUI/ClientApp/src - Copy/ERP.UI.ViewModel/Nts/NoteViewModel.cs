using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class NoteViewModel : TemplateViewModelBase
    {

        /// <summary>
        /// This is used in versioning
        /// </summary>
        public long? NoteId { get; set; }
        public bool? DisableLog { get; set; }


        [Display(Name = "OwnerUserId", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? OwnerUserId { get; set; }
        [Display(Name = "OwnerDisplayName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string OwnerDisplayName { get; set; }
        [Display(Name = "OwnerUserUserName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string OwnerUserUserName { get; set; }

        [Display(Name = "NoteNo", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteNo { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Subject { get; set; }
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Description { get; set; }

        [Display(Name = "TemplateName", ResourceType = typeof(ERP.Translation.Nts.Note))]

        public string TeamName { get; set; }


        [Display(Name = "ReferenceType", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public NoteReferenceTypeEnum? ReferenceType { get; set; }
        [Display(Name = "ReferenceTo", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? ReferenceTo { get; set; }
        [Display(Name = "ReferenceToDisplayName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string ReferenceToDisplayName { get; set; }
        public string FileName { get; set; }
        public bool EnableBroadcast { get; set; }
        public bool SendNotification { get; set; }
        // public NoteReferenceTypeEnum? BroadcastType { get; set; }

        //[Display(Name = "Tag To")]
        //public string BroadcastName { get; set; }


        public NodeEnum? nodeType { get; set; }

        /// <summary>
        /// Actual Start Date
        /// </summary>
        /// 

        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Actual Due Date
        /// </summary>
        [Display(Name = "ExpiryDate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "ReminderDate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ReminderDate { get; set; }

        [Display(Name = "NoteStatusCode", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteStatusCode { get; set; }
        [Display(Name = "NoteStatusName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string NoteStatusName { get; set; }

        public FieldDisplayModeEnum DisplayMode { get; set; }//set runtime
        public SharingModeEnum? SharingMode { get; set; }//set runtime
        public long? Level { get; set; }
        public long? NtsCategoryId { get; set; }

        [Display(Name = "CategoryLevel1", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel1 { get; set; }
        [Display(Name = "CategoryLevel2", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel2 { get; set; }
        [Display(Name = "CategoryLevel3", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel3 { get; set; }
        [Display(Name = "CategoryLevel4", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel4 { get; set; }
        [Display(Name = "CategoryLevel5", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? CategoryLevel5 { get; set; }

        public string CategoryName1 { get; set; }
        public string CategoryName2 { get; set; }
        public string CategoryName3 { get; set; }
        public string CategoryName4 { get; set; }
        public string CategoryName5 { get; set; }
        public long? NoteVersionId { get; set; }

        public long? ReminderId { get; set; }
        public long? RecurreceId { get; set; }

        public long[] TagCategoryId { get; set; }
        public string CSVFileIds { get; set; }
#pragma warning disable CS0108 // 'NoteViewModel.Code' hides inherited member 'TemplateViewModelBase.Code'. Use the new keyword if hiding was intended.
        public string Code { get; set; }
#pragma warning restore CS0108 // 'NoteViewModel.Code' hides inherited member 'TemplateViewModelBase.Code'. Use the new keyword if hiding was intended.

        public long? ShareTo { get; set; }


        [Display(Name = "Attachment", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Attachment { get; set; }

        [Display(Name = "MessageComment", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string MessageComment { get; set; }

        public FileViewModel SelectedFile { get; set; }

        public List<NoteCommentViewModel> Comments { get; set; }

        [Display(Name = "FileId", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? FileId { get; set; }

        public String FormattedCreateDate
        {
            get
            {
                if (CreatedDate.Year == DateTime.Now.Year)
                {
                    return CreatedDate.ToString("MMMM dd") + " at " + CreatedDate.ToString("h:mm tt");
                }
                else
                {
                    return CreatedDate.ToString("MMMM dd yyyy") + " at " + CreatedDate.ToString("h:mm tt");
                }

            }

        }

        public String FormattedStartDate
        {

            get
            {
                DateTime startDate = StartDate ?? DateTime.Now.Date;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMMM dd") + " at " + startDate.ToString("h:mm tt");
                }
                else
                {
                    return startDate.ToString("MMMM dd yyyy") + " at " + startDate.ToString("h:mm tt");
                }

            }

        }

        public String AnnouncementStartDate
        {

            get
            {
                DateTime startDate = StartDate ?? DateTime.Now.Date;
                return startDate.ToString("MMMM dd, yyyy");

            }

        }
        public string PostComment { get; set; }

        public long? S_RequestOverdue { get; set; }
        public long? S_RequestPending { get; set; }

        public long? T_AssignPending { get; set; }
        public long? T_AssignOverdue { get; set; }

        public long? N_Active { get; set; }

        public string base64Img { get; set; }

        public long? PositionId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        public string TargetName { get; set; }
        public string SourceName { get; set; }
        public string Action { get; set; }

        public string LikeStatus { get; set; }

        public long? LikesUserCount { get; set; }

        public long? ILiked { get; set; }

        public long? CommentsCount { get; set; }
        public bool IsAttachmentOpen { get; set; }

        public string MediaAttachmentLocation { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? DisableAutomaticDraft { get; set; }
        public string MediaType
        {
            get
            {

                if (MediaAttachmentLocation != null || CloudDocumentUrl != null)
                {
                    if (CloudDocumentUrl != null && CloudDocumentUrl.Contains("http"))
                    {
                        return "CLOUD";
                    }
                    //if (System.Web.MimeMapping.GetMimeMapping(MediaAttachmentLocation).StartsWith("image/"))
                    //{
                    //    return "PHOTO";
                    //}
                    else if (MediaAttachmentLocation.Contains(".pdf"))
                    {
                        return "PDF";
                    }
                    else
                    {
                        return "VIDEO";
                    }
                }
                return "MESSAGE";
            }
        }


        public string MediaNewType
        {
            get
            {

                if (FileName != null)
                {
                    if (CloudDocumentUrl != null && CloudDocumentUrl.Contains("http"))
                    {
                        return "CLOUD";
                    }
                    //if (System.Web.MimeMapping.GetMimeMapping(FileName).StartsWith("image/"))
                    //{
                    //    return "PHOTO";
                    //}
                    else if (FileName.Contains(".pdf") || FileName.Contains(".PDF"))
                    {
                        return "PDF";
                    }
                    else
                    {
                        return "VIDEO";
                    }
                }
                return "MESSAGE";
            }
        }

        public string StreamVideoPath
        {
            get { return AppSettings.FileWebApiBaseUrl + "api/video"; }
        }

        public string StreamImagePath
        {
            get { return AppSettings.FileWebApiBaseUrl + "api/streamdocs"; }
        }

        public string OwnerFirstLetter
        {
            get { return SourceName != null ? SourceName.First().ToString() : (OwnerDisplayName != null ? OwnerDisplayName.First().ToString() : ""); }
        }

        public bool IsOwner
        {
            get { return OwnerUserId == LoggedInUserId; }
        }

        public string HomeType { get; set; }
        public long? OtherWalls { get; set; }
        public string FollowMode { get; set; }
        public string SourcePost { get; set; }
        public long? FollowerUserId { get; set; }
        public bool? IsUserGuide { get; set; }
        public String CloudDocumentUrl { get; set; }

        public long? ParentId { get; set; }
        public long? PreviousParentId { get; set; }
        public List<long> MultipleReference { get; set; }
        public NodeEnum? MultipleReferenceNodeType { get; set; }
        public ReferenceTypeEnum? MultipleReferenceType { get; set; }
        public long? WorkspaceId { get; set; }

        public string NoteNoLabelName
        {
            get { return NtsNoLabelName.IsNotNullAndNotEmpty() ? NtsNoLabelName : "Note No"; }
        }
        public LockStatusEnum? LockStatus { get; set; }
        public DateTime? LastLockedDate { get; set; }
        public string TagCategoriesCollection { get; set; }
        public string MasterTagCategoriesCollection { get; set; }

        public List<long> TemplateMasterIds { get; set; }

        public DataOperationEvent? DataOperationEvent { get; set; }

        public long? ShareToUser { get; set; }
        public long? ShareToTeamUser { get; set; }

        public bool? UserCanEdit { get; set; }
        public bool? TeamCanEdit { get; set; }

        public bool FirstTime { get; set; }
        public string UploadedContent { get; set; }
        public List<string> Tags { get; set; }

        public bool? EnableBanner { get; set; }
        public long? LogoFileId { get; set; }
        public bool? CanShare { get; set; }

        public long? TemplateBannerId { get; set; }

        public DocumentApprovalStatusEnum? DocumentApprovalStatus { get; set; }
        public DocumentApprovalStatuTypeEnum? DocumentApprovalStatusType { get; set; }

        public string Key { get; set; }
        public string MasterCode { get; set; }

        public string LinkExpiryDate { get; set; }
        public string selectedFiles { get; set; }

        public bool? CanEdit { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public string PermissionType { get; set; }
        public string Access { get; set; }
        public string AppliesTo { get; set; }
        public string AssignTo { get; set; }
        public string TagName { get; set; }
        public bool IsWorkspaceAdmin { get; set; }
        public bool CanManagePermission /*{ get; set; }*/
        {
            get
            {
                if (IsWorkspaceAdmin || IsOwner)
                {
                    return true;
                }
                else if (PermissionType == DmsPermissionTypeEnum.Allow.Description() && (Access == DmsAccessEnum.FullAccess.Description()))
                {
                    return true;
                }

                return false;
            }
        }

        public long? ParentReferenceId { get; set; }
        public ReferenceTypeEnum? ParentReferenceTypeCode { get; set; }
        public NodeEnum? ParentReferenceNode { get; set; }
        public NtsTypeEnum? WorkbookReferenceType { get; set; }
        public long? WorkbookReferenceId { get; set; }

        public List<long> ResourceList { get; set; }
        public bool? IsHelp { get; set; }

        public string NoteOwnerFirstLetter
        {
            get { return (OwnerDisplayName != null && OwnerDisplayName != "") ? OwnerDisplayName.First().ToString() : ((OwnerDisplayName != null && OwnerDisplayName != "") ? OwnerDisplayName.First().ToString() : LoggedInUserName.First().ToString()); }
        }
        public string ExpiryDateDisplay
        {
            get
            {

                var d = ExpiryDate.ToString();
                    return d;
               

            }
        }

        public bool IsOffline { get; set; }
        public bool CanEditDocument
        {
            get
            {
                if (PermissionType == DmsPermissionTypeEnum.Allow.ToString() && (Access == DmsAccessEnum.FullAccess.ToString() || Access == DmsAccessEnum.Modify.ToString()) && (AppliesTo == DmsAppliesToEnum.OnlyThisDocument.ToString() || AppliesTo == DmsAppliesToEnum.ThisFolderAndFiles.ToString() || AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles.ToString()))
                {
                    return true;
                }

                return false;
            }
        }
        public long? PermissionId { get; set; }

        //project fields

        public string ProjectFolder { get; set; }
        public string ProjectSubFolder { get; set; }
        public string Revision { get; set; }
        public string Discipline { get; set; }
       // public string Attachment { get; set; }
        public string GalfarTransmittalNumber { get; set; }
        public string GalfarToQp { get; set; }
        public string OutgoingIssueCodes { get; set; }
        public string DateOfSubmission { get; set; }
        public string QpDueDate { get; set; }
        public string TechnipAttachment1 { get; set; }
        public string QPTransmittalNumber { get; set; }
        public string QpToGalfar { get; set; }
        //public string Code { get; set; }
        public string DateOfReturn { get; set; }
        public string GalfarDueDate { get; set; }
        public string TechnipAttachment2 { get; set; }
        public string StageStatus { get; set; }
        //public string DocumentApprovalStatusType { get; set; }
        public string NativeFileAttachment { get; set; }
       // public string DocumentApprovalStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string LastCheckOutBy { get; set; }
        public string LastCheckOutDate { get; set; }
        public string StepFileIds { get; set; }
        public string TechnipTransmittalNumber { get; set; }
        public string GalfarToTechnip { get; set; }
        public string OutgoingTechnipIssueCodes { get; set; }
        public string OutgoingTransmittalDate { get; set; }
        public string TechnipDueDate { get; set; }
        public string TechnipAttachment3 { get; set; }
        public string CommentAttachment { get; set; }
        public string IncomingTransmittalNumber { get; set; }
        public string TechnipToGalfar { get; set; }
        public string IssueCodes { get; set; }
        public string IncomingTransmittalDate { get; set; }
        public string TechnipAttachment { get; set; }       
        public string TechnipRevisionNo { get; set; }
        public string TechnipDocumentNo { get; set; }
        public string VendorList { get; set; }
        public string VendorDocumentNo { get; set; }
        public string VendorRevisionNo { get; set; }
        public string VendorTransmittalNo { get; set; }
        public string GalfarCommentsToVendor { get; set; }
        public string RevisionNo { get; set; }
        public string VendorOutgoingIssueCode { get; set; }
        public string VendorTransmittalAttachment { get; set; }
        public string InspectionActivity { get; set; }
        public string ContractNo { get; set; }
        public string Date { get; set; }
        public string LocationOfInspection { get; set; }
        public string System { get; set; }
        public string DateOfInspection { get; set; }
        public string OtherDates { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string DrawingNumber { get; set; }
        public string TagNumber { get; set; }
        public string Chainage { get; set; }
        public string ItpQcpNo { get; set; }
        public string ItpItemNo { get; set; }
        public string InterventionPoints { get; set; }
        public string SubContract { get; set; }
        public string Contractor { get; set; }
        public string Qp { get; set; }
        public string Tpi { get; set; }
        public string SubmittedBy { get; set; }
        public string SubContractorName { get; set; }
        public string ContractorName { get; set; }
        public string SubContractorMobileNo { get; set; }
        public string ContractorMobileNo { get; set; }
        public string SubContractDate { get; set; }
        public string ContractDate { get; set; }
        public string CheckedBy { get; set; }
        public string TpiName { get; set; }
        public string ContractorQaQcName { get; set; }
        public string TpiMobileNo { get; set; }
        public string ContractorQaQcMobileNo { get; set; }
        public string TpiContractDate { get; set; }
        public string ContractQaQcDate { get; set; }
        public string FinalRemarks { get; set; }
        public string FooterDocRefNo { get; set; }
        public string QpReferenceNo { get; set; }

        public string ProjectNo { get; set; }
        public string Type { get; set; }
        public string DrawingAttachment { get; set; }

        public long? MyWorkSpaceId { get; set; }
        public long? AdminWorkSpaceId { get; set; }

    }


}


