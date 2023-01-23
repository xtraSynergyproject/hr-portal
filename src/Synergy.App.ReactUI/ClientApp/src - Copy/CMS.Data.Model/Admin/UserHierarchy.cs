using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class UserHierarchy : DataModelBase
    {
        [ForeignKey("HierarchyMaster")]
        public string HierarchyMasterId { get; set; }
        public HierarchyMaster HierarchyMaster { get; set; }

        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }



        [ForeignKey("ParentUser")]
        public string ParentUserId { get; set; }
        public User ParentUser { get; set; }


        public int LevelNo { get; set; }
        public int OptionNo { get; set; }

    }
    [Table("UserHierarchyLog", Schema = "log")]
    public class UserHierarchyLog : UserHierarchy
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
