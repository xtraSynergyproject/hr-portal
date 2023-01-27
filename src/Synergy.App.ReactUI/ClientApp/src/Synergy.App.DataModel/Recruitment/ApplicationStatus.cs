using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationStatus", Schema = "rec")]
    public class ApplicationStatus : DataModelBase
    { 
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
