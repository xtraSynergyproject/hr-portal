using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class PageIndex : DataModelBase
    {
        public IndexRenderingTypeEnum IndexRenderingType { get; set; }
        public bool EnableCreateButton { get; set; }
        public string CreateButtonText { get; set; }
        public string CreateButtonCss { get; set; }
        public CreatePopupTypeEnum CreatePopupType { get; set; }
        public bool EnableEditButton { get; set; }
        public string EditButtonText { get; set; }
        public string EditButtonCss { get; set; }

        public bool EnableDeleteButton { get; set; }
        public string DeleteButtonText { get; set; }
        public string DeleteButtonCss { get; set; }

        public bool EnableDeleteConfirmation { get; set; }
        public string DeleteConfirmationMessage { get; set; }

        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
    }
    [Table("PageIndexLog", Schema = "log")]
    public class PageIndexLog : PageIndex
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
