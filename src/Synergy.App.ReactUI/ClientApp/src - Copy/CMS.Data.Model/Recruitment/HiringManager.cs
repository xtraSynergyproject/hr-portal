using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("HiringManager", Schema = "rec")]
    public class HiringManager : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        //[ForeignKey("Organization")]
        //public string OrganizationId { get; set; }      
        public string Name { get; set; }
        [ForeignKey("Job")]
        public string DesignationId { get; set; }
        public string Email { get; set; }
        public string GAECNo { get; set; }
        [ForeignKey("ListOfValue")]
        public string DepartmentId { get; set; }
    }
}
