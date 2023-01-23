using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class CustomTemplate : DataModelBase
    {
        public bool IsDashboard { get; set; }
        public CustomTemplateTypeEnum CustomTemplateType { get; set; }
        public CustomTemplateLoadingTypeEnum CustomTemplateLoadingType { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string AreaName { get; set; }
        public string Parameter { get; set; }
        public string JavascriptName { get; set; }
        public string JavascriptParam { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }

        public Template Template { get; set; }



        [ForeignKey("LOVType")]
        public string LOVTypeId { get; set; }
        public LOV LOVType { get; set; }
        public string LOVTitle { get; set; }

        public string LOVNameLabel { get; set; }
        public bool IsLOVCodeEnabled { get; set; }
        public string LOVCodeLabel { get; set; }


        public bool IsLOVDescriptionEnabled { get; set; }
        public bool IsLOVDescriptionMandatory { get; set; }
        public string LOVDescriptionLabel { get; set; }
        public bool IsLOVParentEnabled { get; set; }
        public bool IsLOVParentMandatory { get; set; }
        public string LOVParentLabel { get; set; }

        public bool IsLOVIamgeEnabled { get; set; }
        public bool IsLOVIamgeMandatory { get; set; }
        public string LOVImageLabel { get; set; }

        public bool IsLOVStatusEnabled { get; set; }
        public bool IsLOVStatusMandatory { get; set; }
        public string LOVStatusLabel { get; set; }

        public string IconFileId { get; set; }
        public string TemplateColor { get; set; }
        public string BannerFileId { get; set; }
        public string BackgroundFileId { get; set; }
    }
    [Table("CustomTemplateLog", Schema = "log")]
    public class CustomTemplateLog : CustomTemplate
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
