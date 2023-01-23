using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("JobCriteriaTrack", Schema = "rec")]
    public class JobCriteriaTrack : DataModelBase
    {
        [ForeignKey("JobAdvertisementTrack")]
        public string JobAdvertisementTrackId { get; set; }
        public string Criteria { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
        [ForeignKey("ListOfValue")]
        public string CriteriaType { get; set; }
    }
}
