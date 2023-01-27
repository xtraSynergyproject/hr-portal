using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationStateComment", Schema = "rec")]
    public class ApplicationStateComment : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        [ForeignKey("ApplicationState")]
        public string ApplicationStateId { get; set; }
        public ApplicationState ApplicationState { get; set; }       
        public string Comment { get; set; }
    }
}
