using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DMSUserAuditReportViewModel : ViewModelBase
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime? LastLoggedDate { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserRole { get; set; }
        public string MobileNo { get; set; }
        public string WorkSpace { get; set; }

        public string ActivityPerformed { get; set; }

        [DisplayFormat( DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime?  EventDate { get; set; }

        public string  EventName { get; set; }

        public string DocumentName { get; set; }
     
        public long DocumentNoteId { get; set; }
        public long DocumentId { get; set; }
        

        public string DocumentCreatedBy { get; set; }
        public string CheckedOutByUsrName { get; set; }
        public string DocumentType { get; set; }
        public string EventPerformedby { get; set; }

        public string WorkSpaceLegalEntity { get; set; }

        public string WorkspaceName { get; set; }
        public string Folder { get; set; }
  public string LockStatus { get; set; }
  public string LastLockedDate { get; set; }
  public string DocumentCreatedDate { get; set; }
  public string DocumentDescription { get; set; }
    }
}
