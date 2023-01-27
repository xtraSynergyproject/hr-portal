using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationState", Schema = "rec")]
    public class ApplicationState : DataModelBase
    { 
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
