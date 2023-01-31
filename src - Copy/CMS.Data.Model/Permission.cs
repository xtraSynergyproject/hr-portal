using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Permission : DataModelBase
    {
        //[Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Group Name is required.")]
        public string GroupName { get; set; }

        //[Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }
        /// <summary>
        /// PageTypeEnum
        /// </summary>

       // [Required(ErrorMessage = "Page type is required.")]
        public string[] PageTypes { get; set; }
        /// <summary>
        /// UserPermissionTypeEnum
        /// </summary>

       // [Required(ErrorMessage = "User permission type is required.")]
        public string[] UserPermissionTypes { get; set; }


        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }

    }
    [Table("PermissionLog", Schema = "log")]
    public class PermissionLog : Permission
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
