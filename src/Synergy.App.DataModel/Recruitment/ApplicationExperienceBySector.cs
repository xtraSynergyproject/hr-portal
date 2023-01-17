using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("ApplicationExperienceBySector", Schema = "rec")]
    public class ApplicationExperienceBySector : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        [ForeignKey("ListOfValues")]
        public string Sector { get; set; }
        [ForeignKey("ListOfValues")]
        public string Industry { get; set; }
        [ForeignKey("ListOfValues")]
        public string Category { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
