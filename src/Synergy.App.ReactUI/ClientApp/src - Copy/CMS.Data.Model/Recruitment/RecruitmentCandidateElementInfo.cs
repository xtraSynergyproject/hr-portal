using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("RecruitmentCandidateElementInfo", Schema = "rec")]
    public class RecruitmentCandidateElementInfo : DataModelBase
    {
        [ForeignKey("RecruitmentPayElement")]
        public string ElementId { get; set; }
        public RecruitmentPayElement RecruitmentPayElement { get; set; }        
        public double? Value { get; set; }
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
       // public Application Application { get; set; }
    }
}
