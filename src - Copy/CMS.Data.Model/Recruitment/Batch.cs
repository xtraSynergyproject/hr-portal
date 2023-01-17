using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("Batch", Schema = "rec")]
    public class Batch : DataModelBase
    {
        [ForeignKey("Job")]
        public string JobId { get; set; }

        [ForeignKey("Organization")]
        public string OrganizationId { get; set; }
        [ForeignKey("User")]
        public string HiringManager { get; set; }

        [ForeignKey("User")]
        public string HeadOfDepartment { get; set; }
        public string Name { get; set; }
        [ForeignKey("ListOfValue")]
        public string BatchStatus { get; set; }
        public DateTime? TargetHiringDate { get; set; }
        public int? TargetBatchCount { get; set; }
        public BatchTypeEnum? BatchType { get; set; }
    }
}
