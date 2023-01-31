// Authore:Asif Nasim
// Date: 14 sep 2019
// Description: create new for sales module
using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_Project : NodeBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public long? AttachmentId { get; set; }
        public long? ProposalId { get; set; }
        public SalMeasurementUnitEnum MeasurementUnit { get; set; }
        public SalProjectTypeEnum? ProjectType { get; set; }

    }
    public class R_Project_Country : RelationshipBase
    {

    }
    
}
