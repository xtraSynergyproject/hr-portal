
using ERP.Utility;
using System;
using System.ComponentModel;

namespace ERP.Data.GraphModel
{
    public class DataModelBase
    {

        [CreateOnly]
        [PrimaryKey]
        public virtual long Id { get; set; }
        public virtual Guid? UniqueId { get; set; }
        public virtual long? CompanyId { get; set; }
        public virtual StatusEnum Status { get; set; }
        public virtual long IsDeleted { get; set; }
        public virtual long VelocityId { get; set; }
        public virtual long VelocityObjectId { get; set; }
        public long? LogId { get; set; }
        [CreateOnly]
        public virtual long CreatedBy { get; set; }
        [CreateOnly]
        public virtual DateTime CreatedDate { get; set; }
        public virtual long LastUpdatedBy { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
        public long? LoggedInAsByUserId { get; set; }
        [NotMapped]
        public virtual bool IsActive(DateTime? asofDate = null)
        {
            return Status == StatusEnum.Active && IsDeleted == 0;
        }
        [NotMapped]
        public virtual long? NodeId { get; set; }
        private DateTime? _modifiedDateTime;
        public DateTime ModifiedDateTime
        {
            get
            {
                return _modifiedDateTime ?? DateTime.Now.ToUniversalTime();
            }

            set { _modifiedDateTime = value; }
        }
        public NtsModifiedStatusEnum? ModifiedStatus { get; set; }
    }

}