using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Synergy.App.DataModel
{
    public class UserDataPermission : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }

        [ForeignKey("ColumnMetadata")]
        public string ColumnMetadataId { get; set; }
        public ColumnMetadata ColumnMetadata { get; set; }
        public string[] Values { get; set; }

        public LogicalOperationTypeEnum LogicalOperationType { get; set; }


        [ForeignKey("ColumnMetadata2")]
        public string ColumnMetadataId2 { get; set; }
        public ColumnMetadata ColumnMetadata2 { get; set; }
        public string[] Values2 { get; set; }

   
    }
    [Table("UserDataPermissionLog", Schema = "log")]
    public class UserDataPermissionLog : UserDataPermission
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
