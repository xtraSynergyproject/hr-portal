using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("HeadOfDepartment", Schema = "rec")]
    public class HeadOfDepartment : DataModelBase
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; } 
        public string Name { get; set; }
        [ForeignKey("Job")]
        public string DesignationId { get; set; }
        public string Email { get; set; }
        public string GAECNo { get; set; }
        [ForeignKey("ListOfValue")]
        public string DepartmentId { get; set; }
    }
}
