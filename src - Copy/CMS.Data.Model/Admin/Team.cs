using CMS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Team : DataModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string LogoId { get; set; }
        public string GroupCode { get; set; }
        public WorkAssignmentTypeEnum TeamWorkAssignmentType { get; set; }
        public TeamTypeEnum TeamType { get; set; }
        public string[] AllowedPortalIds { get; set; }
    }
    [Table("TeamLog", Schema = "log")]
    public class TeamLog : Team
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
