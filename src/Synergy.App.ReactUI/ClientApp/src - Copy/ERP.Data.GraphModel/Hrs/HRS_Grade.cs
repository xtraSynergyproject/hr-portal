using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public partial class HRS_GradeRoot : RootNodeBase
    {

    }
  
    public partial class HRS_Grade : NodeDatedBase
    {
        [NotMapped]
        public long GradeId { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public long? RankNo { get; set; }
        public long? ProbationDays { get; set; }
       

        public ApprovalStatusEnum ApprovalStatus { get; set; }

        public MedicalCardTypeEnum? MedicalCardType { get; set; }
        public TravelClassEnum? TravelClass { get; set; }
        public int? TicketAllowanceInterval { get; set; }
        public double? MedicalInsuranceCost { get; set; }
        public double? PerDiemCost { get; set; }

        public override bool IsActive(DateTime? asofDate = null)
        {
            return base.IsActive(asofDate) && ApprovalStatus == ApprovalStatusEnum.Approved;

        }
    }
    
    

    public class R_GradeRoot : RelationshipDatedBase
    {

    }

    public partial class HRS_Grade_Log : HRS_Grade
    {
        
        
    }
}
