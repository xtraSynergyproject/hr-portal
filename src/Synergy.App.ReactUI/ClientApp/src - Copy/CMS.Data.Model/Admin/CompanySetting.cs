using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class CompanySetting : DataModelBase
      {
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; } 
    }
    [Table("CompanySettingLog", Schema = "log")]
    public class CompanySettingLog : CompanySetting
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
