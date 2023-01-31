using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;
using Synergy.App.DataModel;


namespace Synergy.App.ViewModel
{
    public class MailViewModel: ViewModelBase
    {
        public string MailNo { get; set; }
        public bool? IsRead { get; set; }

        public string From { get; set; }
        public long? FromId { get; set; }

        public string Subject { get; set; }

        
        public DateTime? StartDate { get; set; }
        public NtsPriorityEnum? Priority { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
        public List<string> To { get; set; }
        public string[] ToName { get; set; }
        public string[] CCName { get; set; }
        public string CCDisplay {
            get
            {
                if (CCName.IsNotNull() && CCName.Length > 0)
                    return string.Join(",", CCName);
                else
                    return "";
            }
        }
        public string ToDisplay { get; set; }
        public List<string> CC { get; set; }

        public string Email { get; set; }

        public string Type { get; set; }

        public long? ParentId { get; set; }
        public long? OwnerUserId { get; set; }

        public long? SourceId { get; set; }
        public NtsTypeEnum? Source { get; set; }
        public string MailboxType { get; set; }
        public string Recieved { get; set; }
        public string WBSItem { get; set; }
        public long? WBSItemId { get; set; }
        public string Project { get; set; }
        public long? ProjectId { get; set; }
        public string ParentDescription { get; set; }
    }
}