using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationState", Schema = "rec")]
    public class ApplicationState : DataModelBase
    { 
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
