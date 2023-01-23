using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("JobDescriptionCriteria", Schema = "rec")]
    public class JobDescriptionCriteria : DataModelBase
    {
        [ForeignKey("JobDescription")]
        public string JobDescriptionId { get; set; }
        public JobDescription JobDescription { get; set; }
        public string Criteria { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
        [ForeignKey("ListOfValue")]
        public string CriteriaTypeId { get; set; }
        [ForeignKey("ListOfValue")]
        public string ListOfValueTypeId { get; set; }
    }
}
