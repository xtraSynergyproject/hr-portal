using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationProject", Schema = "rec")]
    public class ApplicationProject : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Currency { get; set; }
        public double? Value { get; set; }
        public string Client { get; set; }
        public string Consultant { get; set; }
        public DateTime? ConstructionPeriodFrom { get; set; }
        public DateTime? ConstructionPeriodTo { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public bool IsLatest { get; set; }
    }
}
