using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonDocumentViewModel
    {
        public string DocumentType { get; set; }
        //public DocumentOwnerTypeEnum DocumentOwnerType { get; set; }
        public string OwnerName { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? ExpiryDate { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? IssueDate { get; set; }
        public string TemplateCode { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceId { get; set; }
        public string Status { get; set; }
        public long? FileId { get; set; }
        public string DepNoteId { get; set; }
        public string NoteNo { get; set; }
        public string NoteId { get; set; }
        public int DocumentCount { get; set; }
        public long? NoteVersionNo { get; set; }
        public long? AttachVersionNo { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
