using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class HybridHierarchy : DataModelBase
    {
        [ForeignKey("HierarchyMaster")]
        public string HierarchyMasterId { get; set; }
        public HierarchyMaster HierarchyMaster { get; set; }
        public string ParentId { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public int LevelId { get; set; }
        public string[] HierarchyPath { get; set; }
        public string BulkRequestId { get; set; }


    }
    [Table("HybridHierarchyLog", Schema = "log")]
    public class HybridHierarchyLog : HybridHierarchy
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }


}
