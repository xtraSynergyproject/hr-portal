using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("JobCriteria", Schema = "rec")]
    public class JobCriteria : DataModelBase
    {
        [ForeignKey("JobAdvertisement")]
        public string JobAdvertisementId { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }
        public string Criteria { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
        [ForeignKey("ListOfValue")]
        public string CriteriaTypeId { get; set; }
        [ForeignKey("ListOfValue")]
        public string ListOfValueTypeId { get; set; }
    }
}
