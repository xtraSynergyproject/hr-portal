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
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class PMS_PerformanceDocumentMasterStage : PmsBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PMSMasterStageStatusEnum? MasterStageStatus { get; set; }
        public long? SequenceNo { get; set; }
    }
    public class R_PerformanceDocumentMasterStage_PerformanceDocumentMaster : RelationshipBase
    {

    }
}