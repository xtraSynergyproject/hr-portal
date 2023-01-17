using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class HybridHierarchyViewModel : HybridHierarchy
    {
        public string Name { get; set; }
        public long DirectChildCount { get; set; }
        public long AllChildCount { get; set; }
        public string NtsId { get; set; }
        public string EmployeeId { get; set; }
        public virtual string HierarchyId { get; set; }
        public virtual string HierarchyRootNodeId { get; set; }
        public virtual string AllowedRootNodeId { get; set; }
        public virtual bool CanAddRootNode { get; set; }
        public virtual long AllowedRootNodeLevel { get; set; }

        public virtual string AsOnDate { get; set; }
        public string RequestSource { get; set; }
        public string Permission { get; set; }
        public string StatusCode { get; set; }
        public bool HasResponibility { get; set; }
        public string PermissionCodes { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string WorkflowStatus { get; set; }
        public string ItemType { get; set; }

        public string CssClass
        {
            get
            {
                return "org-node-1";
            }
        }
        public string TypeIcon
        {
            get
            {
                switch (ReferenceType)
                {
                    case "ROOT":
                        return "Root";
                    case "LEVEL1":
                        return "Department1";
                    case "LEVEL2":
                        return "Department2";
                    case "LEVEL3":
                        return "Department3";
                    case "LEVEL4":
                        return "Department4";
                    case "BRAND":
                        return "Brand";
                    case "MARKET":
                        return "Market";
                    case "PROVINCE":
                        return "Province";
                    case "DEPARTMENT":
                        return "Department1";
                    case "CAREER_LEVEL":
                        return "CareerLevel";
                    case "JOB":
                        return "Job";
                    case "POSITION":
                        return "Brand";
                    case "EMPLOYEE":
                        return "Employee";
                    default:
                        return "Service";

                }
            }
        }
        public string TypeIconTitle
        {
            get
            {
                switch (ReferenceType)
                {
                    case "ROOT":
                        return "Business Hierarchy Root";
                    case "LEVEL1":
                        return "Department 1";
                    case "LEVEL2":
                        return "Department 2";
                    case "LEVEL3":
                        return "Department 3";
                    case "LEVEL4":
                        return "Department 4";
                    case "BRAND":
                        return "Brand";
                    case "MARKET":
                        return "Market";
                    case "PROVINCE":
                        return "Province";
                    case "DEPARTMENT":
                        return "Department";
                    case "CAREER_LEVEL":
                        return "Career Level";
                    case "JOB":
                        return "Job";
                    case "POSITION":
                        return "Position";
                    case "EMPLOYEE":
                        return "Employee";
                    default:
                        return "Service Request";

                }
            }
        }
        public string ItemBorderCss
        {
            get
            {
                switch (StatusCode)
                {
                    case "SERVICE_STATUS_DRAFT":
                        return "lb-draft";
                    case "SERVICE_STATUS_INPROGRESS":
                        return "lb-inprogress";
                    case "SERVICE_STATUS_OVERDUE":
                        return "lb-overdue";
                    case "SERVICE_STATUS_COMPLETE":
                        return "lb-completed";
                    case "SERVICE_STATUS_REJECT":
                        return "lb-rejected";
                    case "SERVICE_STATUS_CANCEL":
                        return "lb-canceled";
                    case "SERVICE_STATUS_CLOSE":
                        return "lb-canceled";
                    default:
                        return "lb-completed";
                }
            }
        }
    }
}
