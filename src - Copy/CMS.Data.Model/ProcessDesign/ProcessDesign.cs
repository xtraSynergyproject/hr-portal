using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ProcessDesign : DataModelBase
    {
        public ProcessDesignTypeEnum ProcessDesignType { get; set; }

      
        public string Name { get; set; }
        public string ProcessDesignHtml { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        public BusinessLogicExecutionTypeEnum BusinessLogicExecutionType { get; set; }


        [ForeignKey("Action")]
        public string ActionId { get; set; }
        public LOV Action { get; set; }



    }
    [Table("ProcessDesignLog", Schema = "log")]
    public class ProcessDesignLog : ProcessDesign
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
