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
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class PMS_PerformanceDocumentStage : PmsBase
    {
        public DateTime? CompletedDate { get; set; }
        public string Comment { get; set; }
        public PMSStageStatusEnum? StageStatus { get; set; }      
    }
  
    public class R_PerformanceDocumentStage_PerformanceDocumentMasterStage : RelationshipBase
    {

    } 
    public class R_PerformanceDocumentStage_PerformanceDocument : RelationshipBase
    {

    }

}
