using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("JobDescription", Schema = "rec")]
    public class JobDescription : DataModelBase
    {       
        [ForeignKey("Job")]
        public string JobId { get; set; }
        //[ForeignKey("Organization")]
        //public string OrganizationId { get; set; }
        public string Description { get; set; }
        public string QualificationId { get; set; } 
        public long? Experience { get; set; }
        [ForeignKey("ListOfValue")]
        public string JobCategoryId { get; set; }      
        public string Responsibilities { get; set; }
     
    }
}
