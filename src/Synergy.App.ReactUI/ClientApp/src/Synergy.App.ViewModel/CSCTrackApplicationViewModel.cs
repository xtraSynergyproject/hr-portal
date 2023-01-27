using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class CSCTrackApplicationViewModel
    {
        public string ServiceId { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public string ApplicationServiceName { get; set; }
        public string ServiceTemplateCode { get; set; }
        public string ApplicantName { get; set; }
        public string CertificateFileId { get; set; }
        public DateTime? DateOfApplication { get; set; }
        public string DateOfApplicationText { get; set; }
        public DateTime? DeliveryDateOfService { get; set; }
        public string DeliveryDateOfServiceText { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueDateText { get; set; }
        public string ServiceStatus { get; set; }
        public string Remarks { get; set; }
        public bool? Acknowledgement { get; set; }
        public bool? ProvisionalCertificate { get; set; }
        public bool? Certificate { get; set; }

    }
}
