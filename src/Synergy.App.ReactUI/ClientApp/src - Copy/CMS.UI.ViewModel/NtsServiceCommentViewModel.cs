using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsServiceCommentViewModel : NtsServiceComment
    {
        public string CommentedByUserName { get; set; }
        public string CommentedToUserName { get; set; }
        public string PhotoId { get; set; }
        public bool IsAddCommentEnabled { get; set; }
        //public string CommentToUserIds { get; set; }
        public List<string> CommentToUserIds { get; set; }
        public string ParentMessage { get; set; }
        public string CommentedFromUserId { get; set; }
        public string FileName { get; set; }

        public string ParentId { get; set; }
        public string id { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
    }
}
