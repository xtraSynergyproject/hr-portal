using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("Agency", Schema = "rec")]
    public class Agency : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Country")]
        public string CountryId { get; set; }      
        public string AgencyName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
    }
}
