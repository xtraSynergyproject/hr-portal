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
    public partial class PMT_ProjectManagement : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
    }
    public class R_ProjectManagement_User : RelationshipBase
    {

    }
}