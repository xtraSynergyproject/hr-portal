using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_Unit : NodeBase
    {
        public string Name { get; set; }        
        public string FloorNumber { get; set; }
        public string Area { get; set; }
        public string BalconyArea { get; set; }
        public string TotalArea { get; set; }
        public int? NoOfBedrooms { get; set; }
        public int? SNO { get; set; }
        // public string Layout { get; set; }
        // public string PaymentPlan { get; set; }
        //public long? Price { get; set; }
        public int? NoOfParking { get; set; }
       // public long DLDFee { get; set; }
       // public long OkoudFee { get; set; }
        //public int? VAT { get; set; }
       // public long? Commissions { get; set; }
        public SalUnitStatusEnum UnitStatus { get; set; }
        public string UnitPrice { get; set; }
        public string UnitRate { get; set; }
        public string UnitType { get; set; }
        public long? UnitStatusUpdatedBy { get; set; }
        public DateTime? UnitStatusUpdatedDate { get; set; }
        public long? AttachmentId { get; set; }
    }
    public class R_Unit_Project : RelationshipBase
    {

    }
    public class R_Unit_File : RelationshipBase
    {

    }
}
