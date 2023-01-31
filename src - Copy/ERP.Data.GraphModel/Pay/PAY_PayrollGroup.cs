
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public partial class PAY_PayrollGroup : NodeBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int StartDay { get; set; }
        public bool IsStartDayPreviousMonth { get; set; }
        public int EndDay { get; set; }

        public int CutOffStartDay { get; set; }
        public bool IsCutOffStartDayPreviousMonth { get; set; }
        public int CutOffEndDay { get; set; }

        public PayrollIntervalEnum? PayrollInterval { get; set; }
        
    }
    public class R_PayrollGroup_LegalEntity_OrganizationRoot : RelationshipBase
    {

    }
    public class R_PayrollGroup_Mapped_ElementRoot : RelationshipBase
    {

    }
}
