using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;

namespace ERP.UI.ViewModel
{
    public class TaskCommentViewModel : ViewModelBase
    {
        //public long TaskCommentId { get; set; }
        public long TaskId { get; set; }
        public long? TaskVersionId { get; set; }
        public bool? IsCommentDelete { get; set; }
        public long? ParentTaskCommentId { get; set; }
        public long? ParentId { get; set; }
        [Display(Name = "Comment", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string Comment { get; set; }
        [Display(Name = "ParentComment", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ParentComment { get; set; }
        public long CommentedByUserId { get; set; }
        [Display(Name = "CommentedByUserUserName", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string CommentedByUserUserName { get; set; }
        [Display(Name = "Attachment", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string Attachment { get; set; }

        public FileViewModel SelectedFile { get; set; }

        [Display(Name = "FileId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long? FileId { get; set; }

        public string MobileView { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }

        public bool? IsInPhysicalPath { get; set; }

        public String FileType {
            get {
                if (ContentType != null && ContentType.Contains("audio") && (IsInPhysicalPath ?? false))
                {
                    return "audio";
                }else if (ContentType != null && ContentType.Contains("video") && (IsInPhysicalPath ?? false))
                {
                    return "video";
                }else if (ContentType != null && ContentType.Contains("image") && (IsInPhysicalPath ?? false))
                {
                    return "image";
                }
                else
                {
                    return "file";
                }
            } }

        public String FormattedCreatedDate
        {

            get
            {
                DateTime startDate = CreatedDate;
                if (startDate.Year == DateTime.Now.Year)
                {
                    return startDate.ToString("MMMM dd") + " at " + startDate.ToString("h:mm tt");
                }
                else
                {
                    return startDate.ToString("MMMM dd yyyy") + " at " + startDate.ToString("h:mm tt");
                }

            }

        }

        public bool IsOwner
        {

            get
            {
                if(LoggedInUserId == CommentedByUserId)
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


    }
}
