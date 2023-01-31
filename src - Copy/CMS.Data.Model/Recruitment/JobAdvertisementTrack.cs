using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("JobAdvertisementTrack", Schema = "rec")]
    public class JobAdvertisementTrack : DataModelBase
    {
        [ForeignKey("JobAdvertisement")]
        public string JobAdvertisementId { get; set; }
        [ForeignKey("Job")]
        public string JobId { get; set; }       
        public string Description { get; set; }
        public string Qualification { get; set; }
        [ForeignKey("Nationality")]
        public string NationalityId { get; set; }
        [ForeignKey("Location")]
        public string LocationId { get; set; }
        public long? NoOfPosition { get; set; }
        public long? Experience { get; set; }
        [ForeignKey("ListOfValue")]
        public string JobCategory { get; set; }
        public string Responsibilities { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? NeededDate { get; set; }
        [ForeignKey("ListOfValue")]
        public string JobAdvertisementStatus { get; set; }
        [ForeignKey("UserRole")]
        public string RoleId { get; set; }
    }
}
