using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
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
