using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class OCRMapping : DataModelBase
    {       
        public string TemplateId { get; set; }
        public string FieldName { get; set; }
        public string Cordinate1 { get; set; }
        public string Cordinate2 { get; set; }
        public string Cordinate3 { get; set; }
        public string Cordinate4 { get; set; }
        public string Cordinate5 { get; set; }
        
    }
    [Table("OCRMappingLog", Schema = "log")]
    public class OCRMappingLog : OCRMapping
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
