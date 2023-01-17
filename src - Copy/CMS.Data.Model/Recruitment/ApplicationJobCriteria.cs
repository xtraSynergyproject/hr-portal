using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationJobCriteria", Schema = "rec")]
    public class ApplicationJobCriteria : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Criteria { get; set; }
        [ForeignKey("ListOfValue")]
        public string CriteriaType { get; set; }
        [ForeignKey("ListOfValue")]
        public string ListOfValueTypeId { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
        public string Value { get; set; }
    }
}
