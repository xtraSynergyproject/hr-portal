using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class AnnouncementViewModel : ViewModelBase
    {
        public long? OrgId { get; set; }
        public long NoteId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Body { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? StartDate { get; set; }
        [Required]
        [Display(Name = "Expiry Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }
        //[Display(Name = "Is Notify By Email")]
        public bool IsNotifyByEmail { get; set; }
        public long? UserId { get; set; }
        public string Attachment { get; set; }

    }
}
