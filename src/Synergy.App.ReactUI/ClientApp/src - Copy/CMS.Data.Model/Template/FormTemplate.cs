using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class FormTemplate : DataModelBase
    {
       
        public bool EnableIndexPage { get; set; }

        public bool EnableSaveButton { get; set; }
        public string SaveButtonText { get; set; }
        public string SaveButtonCss { get; set; }
        public bool EnableBackButton { get; set; }
        public string BackButtonText { get; set; }
        public string BackButtonCss { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("IndexPageTemplate")]
        public string IndexPageTemplateId { get; set; }
        public FormIndexPageTemplate IndexPageTemplate { get; set; }

        public CreateReturnTypeEnum CreateReturnType { get; set; }
        public EditReturnTypeEnum EditReturnType { get; set; }

        public string PreScript { get; set; }
        public string PostScript { get; set; }
        public FormTypeEnum FormType { get; set; }
    }
    [Table("FormTemplateLog", Schema = "log")]
    public class FormTemplateLog : FormTemplate
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
