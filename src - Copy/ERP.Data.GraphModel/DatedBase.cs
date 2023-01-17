
using ERP.Utility;
using System;
using System.ComponentModel;

namespace ERP.Data.GraphModel
{
    public class DatedBase
    {
        [CreateOnly]
        [PrimaryKey]
        public virtual long Id { get; set; }
        public virtual long? CompanyId { get; set; }
        public virtual StatusEnum Status { get; set; }
        public virtual long IsDeleted { get; set; }
        public virtual long VelocityId { get; set; }
        public virtual long VelocityObjectId { get; set; }
    }

}