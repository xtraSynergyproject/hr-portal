using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Synergy.App.DataModel
{
    [Table("UserRoleStageChild", Schema = "public")]
    public class UserRoleStageChild : DataModelBase
    {
        //public string StatusLabelId { get; set; }
        public string StatusLabel { get; set; }
        public string[] StatusCode { get; set; }
        [ForeignKey("UserRoleStageParent")]
        public string InboxStageId { get; set; }
        public UserRoleStageParent InboxStage { get; set; }

    }
    [Table("UserRoleStageChildLog", Schema = "log")]
    public class UserRoleStageChildLog : UserRoleStageChild
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
