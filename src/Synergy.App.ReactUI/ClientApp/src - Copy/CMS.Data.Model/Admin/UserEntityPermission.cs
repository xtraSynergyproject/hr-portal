using CMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Model
{
    public class UserEntityPermission : DataModelBase
    {
        public UserEntityTypeEnum UserEntityType { get; set; }
        public string UserEntityId { get; set; }       
        public EntityModelTypeEnum EntityModelType { get; set; }
        public string EntityModelId { get; set; }

    }
    [Table("UserEntityPermissionLog", Schema = "log")]
    public class UserEntityPermissionLog : UserEntityPermission
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
