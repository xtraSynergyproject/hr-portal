using ERP.Data.Model;
using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{
    public class JobAdvertismentViewModel : ViewModelBase
    {
        [Display(Name = "Job Name")]
        public string jobId { get; set; }
        [Display(Name = "Other Job Name")]
        public string jobName { get; set; }
        [Display(Name = "No Of Position")]
        public string noOfPosition { get; set; }
        [Display(Name = "Job Description")]
        public string jobDescription { get; set; }
        [Display(Name = "Job Type")]
        public string jobType { get; set; }
        [Display(Name = "Experience")]
        public string experience { get; set; }
        [Display(Name = "Qualification")]
        public string qualification { get; set; }
        [Display(Name = "Nationality")]
        public string nationality { get; set; }
        [Display(Name = "Needed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime neededDate { get; set; }
        [Display(Name = "Job Location")]
        public string jobLocationId { get; set; }
        [Display(Name = "Criteria1")]
        public string criteria1 { get; set; }
        [Display(Name = "Weightage1")]
        public string weightage1 { get; set; }
        [Display(Name = "Criteria Type1")]
        public string criteriaType1 { get; set; }
        [Display(Name = "Criteria2")]
        public string criteria2 { get; set; }
        [Display(Name = "Weightage2")]
        public string weightage2 { get; set; }
        [Display(Name = "Criteria Type2")]
        public string criteriaType2 { get; set; }
        [Display(Name = "Criteria3")]
        public string criteria3 { get; set; }
        [Display(Name = "Weightage3")]
        public string weightage3 { get; set; }
        [Display(Name = "Criteria Type3")]
        public string criteriaType3 { get; set; }
        [Display(Name = "Criteria4")]
        public string criteria4 { get; set; }
        [Display(Name = "Weightage4")]
        public string weightage4 { get; set; }
        [Display(Name = "Criteria Type4")]
        public string criteriaType4 { get; set; }
        [Display(Name = "Criteria5")]
        public string criteria5 { get; set; }
        [Display(Name = "Weightage5")]
        public string weightage5 { get; set; }
        [Display(Name = "Criteria Type5")]
        public string criteriaType5 { get; set; }
        [Display(Name = "Skill1")]
        public string skill1 { get; set; }
        [Display(Name = "Skill Weightage1")]
        public string skillweightage1 { get; set; }
        [Display(Name = "Skill Weightage2")]
        public string skillweightage2 { get; set; }
        [Display(Name = "Skill Weightage3")]
        public string skillweightage3 { get; set; }
        [Display(Name = "Skill3")]
        public string skill3 { get; set; }
        [Display(Name = "Skill2")]
        public string skill2 { get; set; }
        [Display(Name = "Skill Weightage4")]
        public string skillweightage4 { get; set; }
        [Display(Name = "Skill4")]
        public string skill4 { get; set; }
        [Display(Name = "Skill5")]
        public string skill5 { get; set; }
        [Display(Name = "Skill Weightage5")]
        public string skillweightage5 { get; set; }
        [Display(Name = "Other Information")]
        public string otherInformation { get; set; }
        [Display(Name = "Other Information1")]
        public string otherInfo1 { get; set; }
        [Display(Name = "Other Information Type1")]
        public string otherInfoType1 { get; set; }
        [Display(Name = "Other Information2")]
        public string otherInfo2 { get; set; }
        [Display(Name = "Other Information Type2")]
        public string otherInfoType2 { get; set; }
        [Display(Name = "Other Information3")]
        public string otherInfo3 { get; set; }
        [Display(Name = "Other Information Type3")]
        public string otherInfoType3 { get; set; }
        [Display(Name = "Other Information4")]
        public string otherInfo4 { get; set; }
        [Display(Name = "Other Information Type4")]
        public string otherInfoType4 { get; set; }
        [Display(Name = "Other Information5")]
        public string otherInfo5 { get; set; }
        [Display(Name = "Other Information Type5")]
        public string otherInfoType5 { get; set; }
        [Display(Name = "Expiry Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }
        public string JobStatus { get; set; }

    }

}


