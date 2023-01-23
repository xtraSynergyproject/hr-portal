using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class OrganizationChartViewModel : ViewModelBase
    {
      
        public virtual long HierarchyId { get; set; }

        public virtual long OrgHierarchyId { get; set; }

        public virtual long? ParentId { get; set; }

        //  public virtual long OrganizationId { get; set; }

        //   public virtual long? ReportingId { get; set; }
        [Display(Name = "Department Name")]
        public virtual string OrganizationName { get; set; }
        [Display(Name = "Department Type")]
        public virtual long OrganizationTypeId { get; set; }
        [Display(Name = "Department Name")]
        public virtual string OrganizationTypeName { get; set; }

        public virtual string CostCenterCode { get; set; }
        public virtual string CostCenter { get; set; }

        public virtual long? CCHolderPositionId { get; set; }

        public virtual string CCHolderTitle { get; set; }

        public virtual string CCHolderName { get; set; }
        public virtual string LocationName { get; set; }

        public virtual long Level { get; set; }


        public virtual bool HasChild { get; set; }
        public virtual long DirectChildCount { get; set; }


        public virtual string ReportingLine { get; set; }

        public virtual bool? IsExpanded { get; set; }

        public virtual bool? Collapsed { get; set; }

        public virtual string CssClass
        {
            get
            {
                return "org-node-1";
            }
        }
       

    }
}
