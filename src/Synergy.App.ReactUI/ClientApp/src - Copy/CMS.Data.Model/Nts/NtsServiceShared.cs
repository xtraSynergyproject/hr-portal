using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsServiceShared : DataModelBase
    {

        [ForeignKey("ServiceSharedWithType")]
        public string ServiceSharedWithTypeId { get; set; }
        public LOV ServiceSharedWithType { get; set; }


        [ForeignKey("SharedWithUser")]
        public string SharedWithUserId { get; set; }
        public User SharedWithUser { get; set; }


        [ForeignKey("SharedWithTeam")]
        public string SharedWithTeamId { get; set; }
        public Team SharedWithTeam { get; set; }


    
        [ForeignKey("SharedBy")]
        public string SharedByUserId { get; set; }
        public User SharedBy { get; set; }



        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }

        public DateTime SharedDate { get; set; }
        public WorkBoardContributionTypeEnum ContributionType { get; set; }

    }
    [Table("NtsServiceSharedLog", Schema = "log")]
    public class NtsServiceSharedLog : NtsServiceShared
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }

}
