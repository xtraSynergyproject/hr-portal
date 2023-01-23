
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public partial class PAY_SalaryElementInfo : NodeBase
    {
        public double Amount { get; set; }
        public bool IsLatest { get; set; }
        public virtual DateTime EffectiveStartDate { get; set; }
        public virtual DateTime EffectiveEndDate { get; set; }
    }

    public class R_SalaryElementInfo_SalaryInfoRoot : RelationshipBase
    {
        
    }
    public class R_SalaryElementInfo_ElementRoot : RelationshipBase
    {

    }
}
