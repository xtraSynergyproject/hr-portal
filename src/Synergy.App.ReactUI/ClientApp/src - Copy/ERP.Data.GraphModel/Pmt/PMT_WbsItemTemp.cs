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


namespace ERP.Data.GraphModel
{
    public partial class PMT_WbsItemTemp : NodeBase
    {
        public string SequenceNo { get; set; }
        public string QpRefernce { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }        
        public string WorkHours { get; set; }                
        public string Priorty { get; set; }               
        public string PlanStartDate { get; set; }        
        public string PlanEndDate { get; set; }        
        public string ActualStartDate { get; set; }        
        public string ActualEndDate { get; set; }       
        public string ForcastStartDate { get; set; }        
        public string ForcastEndDate { get; set; }        
        public string ErrorMessage { get; set; }
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public long FolderId { get; set; }
        public string FolderLoaction { get; set; }
    }
   
}