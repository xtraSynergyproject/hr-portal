using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationStatus", Schema = "rec")]
    public class ApplicationStatus : DataModelBase
    { 
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
