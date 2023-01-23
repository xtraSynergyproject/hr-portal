using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DocumentApprovalViewModel
    {
        
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceStatusCode { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime DueDate { get; set; }
        public string DueDateText { get { return DueDate.ToDefaultDateFormat(); } }
        public long DocumentId { get; set; }
        public long NoteId { get; set; }
        public string DocumentName { get; set; }
        public string ServiceId { get; set; }
        public long? DocumentTypeDashboardId { get; set; }
        public string DocumentTypeName { get; set; }
        public long? WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }

    }
}
