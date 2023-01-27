
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonDocumentViewModel
    {           
        public string DocumentType { get; set; }
        public DocumentOwnerTypeEnum DocumentOwnerType { get; set; }
        public string OwnerName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? IssueDate { get; set; }

        public string ServiceNo { get; set; }
        public long? ServiceId { get; set; }
        public string Status { get; set; }
        public long? FileId { get; set; }

        public string NoteNo { get; set; }
        public long? NoteId { get; set; }
        public int DocumentCount { get; set; }
        public long? NoteVersionNo { get; set; }
        public long? AttachVersionNo { get; set; }
    }
}
