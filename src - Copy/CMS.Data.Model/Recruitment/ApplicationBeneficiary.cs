using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationBeneficiary", Schema = "rec")]
    public class ApplicationBeneficiary : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
       // public Application Application { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Relationship { get; set; }
        public double? Ratio { get; set; }
    }
}
