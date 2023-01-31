using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ResourceLanguage : DataModelBase
    {
        public TemplateTypeEnum? TemplateType { get; set; }
        public ResourceLanguageGroupCodeEnum? GroupCode { get; set; }
        public string TemplateId { get; set; }
        public string Code { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        public string EnglishHelperText { get; set; }
        public string ArabicHelperText { get; set; }
        public string EnglishTooltip { get; set; }
        public string ArabicTooltip { get; set; }
    }
    [Table("ResourceLanguageLog", Schema = "log")]
    public class ResourceLanguageLog : ResourceLanguage
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }
}
