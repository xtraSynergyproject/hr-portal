using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class CustomIndexPageTemplate : DataModelBase
    {
        public bool EnableCreateButton { get; set; }
        public string CreateButtonText { get; set; }
        public string CreateButtonCss { get; set; }
        public bool EnableEditButton { get; set; }
        public string EditButtonText { get; set; }
        public string EditButtonCss { get; set; }

        public bool EnableDeleteButton { get; set; }
        public string DeleteButtonText { get; set; }
        public string DeleteButtonCss { get; set; }

        public bool EnableDeleteConfirmation { get; set; }
        public string DeleteConfirmationMessage { get; set; }

        public bool EnableExportToExcel { get; set; }
        public bool EnableExportToPdf { get; set; }


        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("OrderByColumn")]
        public string OrderByColumnId { get; set; }
        public ColumnMetadata OrderByColumn { get; set; }
        public OrderByEnum OrderBy { get; set; }
        public string TemplateSelectionPopupTitle { get; set; }
    }
    [Table("CustomIndexPageTemplateLog", Schema = "log")]
    public class CustomIndexPageTemplateLog : CustomIndexPageTemplate
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
