//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class HRS_Nationality : NodeBase
    {
        public string Code { get; set; }
        public bool IsNational { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public double? AverageEconomyTicketCost { get; set; }
        public double? AverageBusinessTicketCost { get; set; }

    }

    public partial class HRS_Nationality_Log : HRS_Nationality
    {        

    }
}
