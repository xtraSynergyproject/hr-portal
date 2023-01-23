using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class ServiceSearchViewModel 
    {
        public string Id { get; set; }
       // public bool ForAllCompany { get; set; }          
        public virtual StatusEnum? Status { get; set; }
        public DataOperation? Operation { get; set; }
        public string ServiceId { get; set; }       
        public string ServiceNo { get; set; }       
        public string Subject { get; set; }      
        public DateTime? StartDate { get; set; }
      
        public DateTime? StartToDate { get; set; }
       
        public DateTime? DueDate { get; set; }
        public DateTime? DueToDate { get; set; }
       
        public LOVViewModel AssignedToType { get; set; }
      
        public DateTime? CreationDate { get; set; }
       
        public DateTime? CompletionDate { get; set; }
        public DateTime? CompletionToDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string ServiceStatus { get; set; }
      
        public string RequestedBy { get; set; }
        public string Mode { get; set; }
        public string ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ViewType { get; set; }
        public string OwnerUserId { get; set; }
        public string UserId { get; set; }
        public string FilterUserId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TemplateCategoryCode { get; set; }
        public TemplateCategoryTypeEnum? TemplateCategoryType { get; set; }
        public string Text { get; set; }
        public string RequestSource { get; set; }
        public string Layout { get; set; }
        public string ReturnUrl { get; set; }
        public string TemplateMasterId { get; set; }
        public NtsTypeEnum? NTSType { get; set; }
        public List<ModuleViewModel> ModuleList { get; set; }
        public List<string> ServiceStatusIds { get; set; }
        public List<string> StatusIds { get; set; }
        public List<NtsUserTypeEnum> UserType { get; set; }
        public string PortalNames { get; set; }
        public string TemplateDisplayName { get; set; }
    }
   public class ServiceViewModelComparer : IEqualityComparer<ServiceViewModel>
    {
        public bool Equals(ServiceViewModel x, ServiceViewModel y)
        {
            if (x.Id == y.Id)
                return true;

            return false;
        }

        public int GetHashCode(ServiceViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
