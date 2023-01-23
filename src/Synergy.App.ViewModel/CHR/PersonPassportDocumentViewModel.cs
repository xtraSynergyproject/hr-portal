﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PersonPassportDocumentViewModel : PersonDocumentViewModel
    {

        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Display(Name = "Issue Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? PassIssueDate { get; set; }
        [Display(Name = "Expiry Date")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? PassExpiryDate { get; set; }
        [Display(Name = "Place Of Issue")]
        public string PlaceOfIssue { get; set; }
        [Display(Name = "Nationality")]
        public long? NationalityId { get; set; }
        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Place Of Birth")]
        public string PlaceOfBirth { get; set; }
        [Display(Name = "Birth Country")]
        public long? BirthCountry { get; set; }
        [Display(Name = "Birth Country")]
        public string BirthCountryName { get; set; }
        public string PassportAttachment { get; set; }

    }
}