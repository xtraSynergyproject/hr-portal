using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsTaskCommentViewModel : NtsTaskComment
    {
        public string CommentedByUserName { get; set; }
        public string CommentedToUserName { get; set; }
        public string PhotoId { get; set; }
        public bool IsAddCommentEnabled { get; set; }
        public int pageNumber { get; set; }
        public string pageNumberDisplay { get { return pageNumber == 0 ? "" : pageNumber.ToString(); } }

        public string FileId { get; set; }
        public string FileName { get; set; }
        public List<NtsTaskCommentViewModel> comments { get; set; }
        public string textMarkupAnnotation { get; set; }
        public string author { get; set; }
        public DateTime modifiedDate { get; set; }

        public string note { get; set; }
        public string commentsDisplay { get { return note.IsNotNullAndNotEmpty() ? note : dynamicText; } }
        public string annotName { get; set; }
        public string parentId { get; set; }
        public string subject { get; set; }
        public string dynamicText { get; set; }
        public List<string> CommentToUserIds { get; set; }

        public string ParentMessage { get; set; }
        public string CommentedFromUserId { get; set; }
        public string ParentId { get; set; }
        public string id { get; set; }
        public bool hasChildren { get; set; }
        public bool expanded { get; set; }
    }
}

    
