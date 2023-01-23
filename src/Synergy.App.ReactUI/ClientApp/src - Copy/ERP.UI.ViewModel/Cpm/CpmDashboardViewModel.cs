using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CpmDashboardViewModel : ViewModelBase
    {
        [Display(Name = "Payment Category")]        
        public string PaymentCategory { get; set; }
        public string PaymentId { get; set; }
        public string ContractId { get; set; }
        public string TenantId { get; set; }
        public string ProjectName { get; set; }
        public string UnitNumber { get; set; }
        public string TenantName { get; set; }
        public string PaymentType { get; set; }        
        public string PaymentStatus { get; set; }
        public string PaymentAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}")]
        public string PaymentDueDate { get; set; }

        //[Display(Name = "Bounced Date ")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        //public DateTime? BouncedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}")]
        public string BouncedDate { get; set; }
        public int UpcomingPaymentCount { get; set; }
        public int OverduePaymentCount { get; set; }
        public int EndContractCount { get; set; }
        public int MaintenanceRequestCount { get; set; }
        public int ReminderCount { get; set; }
        public int BouncedPaymentCount { get; set; }
        public int ExpiredDocumentCount { get; set; }
        public int OverdueContractCount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public string ContractEndDate { get; set; }
        public string TenantContactNo { get; set; }
        public string MaintenanceRequestId { get; set; }
        public string MaintenanceTitle { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public string  MaintenanceRequestTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}")]
        public string PassportExpiryDate { get; set; }

        public string ReminderId { get; set; }
        public string Title { get; set; }
        public string ToDo { get; set; }
        public string AssignToName { get; set; }
        public string Priority { get; set; }
    }
}
