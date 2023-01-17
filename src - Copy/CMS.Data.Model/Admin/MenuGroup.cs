using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class MenuGroup : DataModelBase
    {
        //[Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string MenuGroupDetails { get; set; }

        //[Required(ErrorMessage = "Short name is required.")]
        public string ShortName { get; set; }

        [ForeignKey("SubModule")]
        [Required(ErrorMessage = "Sub module is required.")]
        public string SubModuleId { get; set; }

        public SubModule SubModule { get; set; }
        public string ParentId { get; set; }

        public string IconCss { get; set; }
        public string IconColor { get; set; }


        public Portal Portal { get; set; }

        [ForeignKey("Portal")]       
        public string PortalId { get; set; }
        public string MenuGroupIconFileId { get; set; }
        public bool ExpandHelpPanel { get; set; }

    }
    [Table("MenuGroupLog", Schema = "log")]
    public class MenuGroupLog : MenuGroup
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
