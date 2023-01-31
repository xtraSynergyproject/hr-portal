using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCEnforcementUnAuthorizationViewModel : ViewModelBase
    {
        public string WardNo { get; set; }
        public string TypesOfViolation { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CaseId { get; set; }
        public string DetailsofViolation { get; set; }
        public string Attachments { get; set; }
        public List<IdNameViewModel> AttachmentList { get; set; }
        public List<IdNameViewModel> TypesOfViolationList { get; set; }
        public string ReportedDateTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AuthorizationId { get; set; }
        public string OrderNo { get; set; }
        public string ApprovalDetails { get; set; }
        public string ApprovalDate { get; set; }
        public string PortalName { get; set; }
        public string Remarks { get; set; }
        public string SubLoginType { get; set; }
    }
}
