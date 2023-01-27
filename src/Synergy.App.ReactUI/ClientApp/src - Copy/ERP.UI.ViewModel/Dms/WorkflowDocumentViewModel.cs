using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class WorkflowDocumentViewModel:ViewModelBase
    {
        [Display(Name = "Document Workflow")]
        public long DocumentWorkflowId { get; set; }

        [Display(Name = "Document Workflow")]
        public long[] WorkflowId { get; set; }
        public string DocumentWorkflowName { get; set; }       
        public long DocumentId { get; set; }     
        public long? PreApprovalFolderId { get; set; }
        public long? Draft_SubmitFolderId { get; set; }
        public long? InprogressFolderId { get; set; }
        public long? CompletedFolderId { get; set; }
        public long? CanceledFolderId { get; set; }
        public string PreApprovalFolder { get; set; }
        public string Draft_SubmitFolder { get; set; }
        public string InprogressFolder { get; set; }
        public string CompletedFolder { get; set; }
        public string CanceledFolder { get; set; }
        [Required]
        [Display(Name = "Action")]
        public ActionEnum? ActionType { get; set; }
    }
   
}
