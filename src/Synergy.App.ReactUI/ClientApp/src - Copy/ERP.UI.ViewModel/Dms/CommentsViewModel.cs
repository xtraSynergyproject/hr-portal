using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class CommentsViewModel 
    {
        public string textMarkupAnnotation { get; set; }
        [Display(Name = "Author")]
        public string author { get; set; }
        [Display(Name = "Modified Date")]
        [DisplayFormat(DataFormatString = Constant.Annotation.LongDateTimeFormat)]
        public DateTime modifiedDate { get; set; }
        
        public string note { get; set; }
        public string annotName { get; set; }
        public string parentId { get; set; }
        [Display(Name = "Comment Type")]
        public string subject { get; set; }
        public string dynamicText { get; set; }
        [Display(Name = "Message")]
        public string commentsDisplay { get { return note.IsNotNullAndNotEmpty() ? note : dynamicText; } }

        [Display(Name = "Page No")]
        public int pageNumber { get; set; }
        [Display(Name = "Page Number")]
        public string pageNumberDisplay { get { return pageNumber == 0 ? "" : pageNumber.ToString(); } }
        public List<CommentsViewModel> comments { get; set; }

        public long? FileId { get; set; }
        public string FileName { get; set; }
    }
}
