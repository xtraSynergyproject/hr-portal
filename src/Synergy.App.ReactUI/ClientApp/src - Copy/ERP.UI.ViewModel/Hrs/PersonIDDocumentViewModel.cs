﻿
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonIDDocumentViewModel :PersonDocumentViewModel
    {
        [Display(Name = "ID Type")]
        public string IDType { get; set; }
        public string IDTypeCode { get; set; }
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        [Display(Name = "Issue Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? IDIssueDate { get; set; }
        [Display(Name = "Hijiri Issue Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? IDHijiriIssueDate { get; set; }
        [Display(Name = "Hijiri Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? IDHijiriExpiryDate { get; set; }
        [Display(Name = "ID Card Job Title")]
        public string IDCardJobTitle { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }
        public string IDAttachmentFront { get; set; }
        public string IDAttachmentBack { get; set; }
    }
}
