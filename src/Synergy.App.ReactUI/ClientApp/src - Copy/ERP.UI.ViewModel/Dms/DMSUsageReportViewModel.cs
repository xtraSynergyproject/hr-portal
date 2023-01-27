using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class DMSUsageReportViewModel : ViewModelBase
    {
        public string DocumentName { get; set; }
        public long UserId { get; set; }
        public long DocumentNoteId { get; set; }
        public long DocumentId { get; set; }
      //  public string CheckedInBy { get; set; }
      //  public string CheckedOutBy { get; set; }
        public string ViewedBy { get; set; }
         public string StatusName { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DocumentCreatedBy { get; set; }
        public string CheckedOutByUsrName { get; set; }
        public string DocumentType { get; set; }
        public LockStatusEnum? DocumentlockStatus { get; set; }
        public DateTime? LastLockedDate { get; set; }
        //public string WrkSpcTags { get; set; }
        public string[] WrkSpcTags { get; set; }
        public string WrkSpcTag
        {
            get
            {
                var str = "";
                if (WrkSpcTags != null)
                    str = string.Join(",", WrkSpcTags);
                return str;
            }
        }
        public string WorkSpaceLegalEntity { get; set; }

        public string WorkspaceName { get; set; }
        public string Folder { get; set; }
        public string ActivityPerformed { get; set; }

        [DisplayFormat(DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? EventDate { get; set; }

        public string EventName { get; set; }
        public string EventPerformedby { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestUpdatedDate { get; set; }

        public string RequestUpdatedByName { get; set; }
        public string RequestStatus { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestStartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestDueDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestEndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? RequestCompletedDate { get; set; }
    }
}
