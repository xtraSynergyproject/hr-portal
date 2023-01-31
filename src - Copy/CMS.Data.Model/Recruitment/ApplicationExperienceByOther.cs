using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationExperienceByOther", Schema = "rec")]
    public class ApplicationExperienceByOther : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        [ForeignKey("ListOfValues")]
        public string OtherTypeId { get; set; }
        public double? NoOfYear { get; set; }
        public bool IsLatest { get; set; }
    }
}
