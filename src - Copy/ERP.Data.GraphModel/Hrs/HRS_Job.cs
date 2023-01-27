//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class HRS_JobRoot : RootNodeBase
    {

    }
  
    public partial class HRS_Job : NodeDatedBase
    {
        [NotMapped]
        public long JobId { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public ApprovalStatusEnum ApprovalStatus { get; set; }

        public override bool IsActive(DateTime? asofDate = null)
        {
            return base.IsActive(asofDate) && ApprovalStatus == ApprovalStatusEnum.Approved;

        }
    }
    public class R_JobRoot : RelationshipDatedBase
    {

    }
    public class R_Job_GradeRoot : RelationshipBase
    {

    }

    public partial class HRS_Job_Log : HRS_Job
    {
        public string GradeName { get; set; }
    }
}