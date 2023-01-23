using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCPaymentViewModel: ViewModelBase
    {
        public string TaskId { get; set; }
        public string ServiceId { get; set; }
        public string PaymentSubject { get; set; }
        public string Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? DueDate { get; set; }        
        public DateTime? PaymentFromDate { get; set; }        
        public DateTime? PaymentToDate { get; set; }        
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentError { get; set; }
        public string ReferenceNo { get; set; }
        public string NoteOwnerUserId { get; set; }
        public string TaskNo { get; set; }
        public string ServiceNo { get; set; }
        public string OwnerUserName { get; set; }
        public string TaskStatusName { get; set; }
        public string PaymentStatusCode { get; set; }
        public string PaymentIds { get; set; }
        public string TotalAmount { get; set; }
        public string NoteId { get; set; }
        public string TemplateCode { get; set; }
        public string SourceReferenceId { get; set; }
        public string PaymentModeName { get; set; }
        public string PaymentStatusName { get; set; }
        public string OwnerName { get; set; }
        public string SourceReferenceType { get; set; }
        public string pcl_id { get; set; }
        public string prop_id { get; set; }
        public string RevenueTypeName { get; set; }
        public string RevenueTypeId { get; set; }
    }
}
