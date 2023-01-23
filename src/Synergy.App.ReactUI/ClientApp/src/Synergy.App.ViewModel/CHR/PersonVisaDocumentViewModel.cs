﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonVisaDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Visa Number")]
        public string VisaNumber { get; set; }
        [Display(Name = "UID No")]
        public string UIDNo { get; set; }
        [Display(Name = "Duration Of Stay")]
        public string DurationOfStay { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }
        [Display(Name = "Visa Type")]
        public VisaTypeEnum? VisaType { get; set; }
        [Display(Name = "Entry Type")]
        public VisaEntryTypeEnum? EntryType { get; set; }
        [Display(Name = "Sponsor Name")]
        public string SponsorName { get; set; }
        [Display(Name = "Purpose")]
        public string Purpose { get; set; }
        [Display(Name = "Visa Job Title")]
        public string VisaJobTitle { get; set; }
        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? VisaIssueDate { get; set; }
        [Display(Name = "Expiry Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? VisaExpiryDate { get; set; }
        public string VisaAttachment { get; set; }

    }
}
