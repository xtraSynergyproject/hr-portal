﻿using CMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Model
{
    public class UserPermission : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Page")]
        public string PageId { get; set; }
        public Page Page { get; set; }
        public string[] Permissions { get; set; }
    }
    [Table("UserPermissionLog", Schema = "log")]
    public class UserPermissionLog : UserPermission
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