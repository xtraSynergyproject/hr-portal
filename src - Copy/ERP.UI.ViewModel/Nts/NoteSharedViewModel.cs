using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class NoteSharedViewModel : ViewModelBase
    {
        public long NoteSharedId { get; set; }
        public long NoteId { get; set; }
        [Display(Name = "SharedType", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public AssignToTypeEnum? SharedType { get; set; }
        public long? SharedTo { get; set; }
        [Display(Name = "SharedToUserName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string SharedToUserName { get; set; }
        public long? TeamId { get; set; }
        
        public bool? FullAccess { get; set; }
        public bool? CanView { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanShare { get; set; }
        public long? SharedByUserId { get; set; }
        public bool IsSharedUser { get; set; }

    }
    public class Permission
    {
        public long UserId { get; set; }
        public bool Value { get; set; }
        public string CheckBoxName { get; set; }
        public AssignToTypeEnum ShareType { get; set; }
    }

    public class ExternalLinkViewModel {
        //long userId, long noteId, long linkId, string emailTo, string link, DateTime? expirydate
        public long NoteId { get; set; }
        public long LinkId { get; set; }
        public long FromUserId { get; set; }
        public string SenderName { get; set; }
        public string EmailTo { get; set; }
        public string EmailLink { get; set; }
        public DateTime? Expirydate { get; set; }

    }
}
