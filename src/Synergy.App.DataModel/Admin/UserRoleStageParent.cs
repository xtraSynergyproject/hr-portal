using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.DataModel
{
    [Table("UserRoleStageParent", Schema = "public")]
    public class UserRoleStageParent : DataModelBase
    {
        //public string InboxStageId { get; set; }
        public string InboxStageName { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateShortName { get; set; }
        public string TemplateName { get; set; }
        [ForeignKey("UserRole")]
        public string UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public string StageSequence { get; set; }
        public string ChildSequence { get; set; }
        public string InboxCode { get; set; }
    }
    [Table("UserRoleStageParentLog", Schema = "log")]
    public class UserRoleStageParentLog : UserRoleStageParent
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
