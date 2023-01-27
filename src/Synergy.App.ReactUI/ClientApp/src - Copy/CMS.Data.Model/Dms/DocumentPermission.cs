using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Model
{
    public class DocumentPermission : DataModelBase
    {
        public DmsPermissionTypeEnum PermissionType { get; set; }
        public DmsAccessEnum Access { get; set; }       
        public DmsAppliesToEnum AppliesTo { get; set; }        

        [ForeignKey("NtsNote")]
        public string NoteId { get; set; }
        public NtsNote NtsNote { get; set; }
        [ForeignKey("User")]
        public string PermittedUserId { get; set; }
        public User User { get; set; }
        [ForeignKey("UserGroup")]
        public string PermittedUserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }
        public bool Isowner { get; set; }
       
        public bool IsInherited { get; set; }
        public bool IsInheritedFromChild { get; set; }
        public string InheritedFrom { get; set; }
    }
    [Table("DocumentPermissionLog", Schema = "log")]
    public class DocumentPermissionLog : DocumentPermission
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
