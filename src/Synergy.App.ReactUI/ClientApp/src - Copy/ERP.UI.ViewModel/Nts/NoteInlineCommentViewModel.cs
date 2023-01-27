using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class NoteInlineCommentViewModel : ViewModelBase
    {
        //public long NoteCommentId { get; set; }
        public long NoteId { get; set; }
        public long? NoteVersionId { get; set; }
        public long? ParentNoteCommentId { get; set; }
        public long? ParentId { get; set; }
        //[Display(Name = "Message")]
        [Display(Name = "Comment", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Comment { get; set; }
        //[Display(Name = "Message")]
        [Display(Name = "ParentComment", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string ParentComment { get; set; }
        public long CommentedByUserId { get; set; }
        //[Display(Name = "Created By")]
        [Display(Name = "CommentedByUserUserName", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string CommentedByUserUserName { get; set; }
        //[Display(Name = "Attachment")]
        [Display(Name = "Attachment", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string Attachment { get; set; }
        public bool? IsCommentDelete { get; set; }
        public string CommentsCount { get; set; }

        public FileViewModel SelectedFile { get; set; }

        //[Display(Name = "Document Upload")]
        [Display(Name = "FileId", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long? FileId { get; set; }
        public string MobileView { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }

        public bool? IsInPhysicalPath { get; set; }

        public String FileType
        {
            get
            {
                if (ContentType != null && ContentType.Contains("audio") && (IsInPhysicalPath ?? false))
                {
                    return "audio";
                }
                else if (ContentType != null && ContentType.Contains("video") && (IsInPhysicalPath ?? false))
                {
                    return "video";
                }
                else if (ContentType != null && ContentType.Contains("image") && (IsInPhysicalPath ?? false))
                {
                    return "image";
                }
                else
                {
                    return "file";
                }
            }
        }
        public String FormattedCreateDate
        {
            get
            {
                if (CreatedDate.Year == DateTime.Now.Year)
                {
                    return CreatedDate.ToString("MMMM dd") + " at " + CreatedDate.ToString("h:mm tt");
                }
                else
                {
                    return CreatedDate.ToString("MMMM dd yyyy") + " at " + CreatedDate.ToString("h:mm tt");
                }

            }

        }

        public string CommenterFirstLetter
        {
            get { return CommentedByUserUserName != null ? CommentedByUserUserName.First().ToString() : ""; }
        }
        public bool IsOwner
        {

            get
            {
                if (LoggedInUserId == CommentedByUserId)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
        public string CSVFileIds { get; set; }
        public long? PhotoId { get; set; }
    }
}
