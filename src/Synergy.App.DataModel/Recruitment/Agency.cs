using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
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
