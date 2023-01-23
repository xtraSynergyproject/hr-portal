
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;

namespace CMS.UI.ViewModel
{
    public class JobViewModel:ViewModelBase
    {
        public long JobId { get; set; }
        //[Required]
       // [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[StringLength(200)]
       // [StringLength(Constant.NameStringLength1, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Job Title")]
        //[Display(Name = "Name", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public string Name { get; set; }
        public string JobTitle { get; set; }

        //[StringLength(200)]
        //  [StringLength(Constant.NameStringLength1, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Job Title (In Arabic)")]
        //  [Display(Name = "NameLocal", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public string NameLocal { get; set; }

        //[StringLength(2000)]
       // [StringLength(Constant.LongStringLength, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
      //  [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public string Description { get; set; }

        //[StringLength(2000)]
      //  [StringLength(Constant.LongStringLength, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
      //  [Display(Name = "Comments", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public string Comments { get; set; }
    //    [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public int? SequenceNo { get; set; }
        //[Required]
      //  [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        //[Display(Name = "Grade Name")]
     //   [Display(Name = "GradeId", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public long? GradeId { get; set; }

        //[Display(Name = "Grade Name")]
      //  [Display(Name = "GradeName", ResourceType = typeof(ERP.Translation.Hrs.Job))]
        public string GradeName { get; set; }

        [Display(Name = "Attachment")]
        public long? JobAttachmentId { get; set; }
        public FileViewModel JobSelectedFile { get; set; }
        public string NewName { get; set; }
        public long? PositionId { get; set; }
    }
}
