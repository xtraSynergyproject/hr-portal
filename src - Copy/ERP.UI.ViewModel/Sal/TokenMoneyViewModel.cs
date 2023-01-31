using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.Web;

namespace ERP.UI.ViewModel
{
    public class TokenMoneyViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }
        [Required]
        [Display(Name = "Booking Amount")]
        public long? TokenAmount { get; set; }
        [Required]
        [Display(Name = "Payment Mode")]
        public SalPaymentModeEnum PaymentMode { get; set; }
        //[Required]       
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Booking Date")]
        public DateTime? PaymentDate { get; set; }
        [Required]
        [Display(Name = "Lead Name")]
        public long LeadId { get; set; }
        [Display(Name = "Lead Name")]
        public string LeadName { get; set; }
        public long? LeadPersonId { get; set; }
        public string LeadPersonName { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public long ProjectId { get; set; }
        [Display(Name = "Project Name")]
        
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Unit Type Name")]
        public long UnitTypeId { get; set; }
        [Display(Name = "Unit Type Name")]
        
        public string UnitTypeName { get; set; }
        [Required]
        [Display(Name = "Unit Number")]
        public long UnitId { get; set;}
        [Display(Name = "Unit Number")]
        public long[] UnitIdData { get; set; }
        [Display(Name = "Unit Number")]
        public string UnitName { get; set; }
        public SalUnitStatusEnum UnitStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? UnitStatusDate { get; set; }
        public string UnitPrice { get; set; }
        public long? ServiceId { get; set; }       

      
        public FileViewModel OtherDocumentsSelectedFile { get; set; }
        [Required]
        public string Attachments { get; set; }

        [Display(Name = "Currency")]
        public long CurrencyId { get; set; }
        [Display(Name = "Currency")]
        public string CurrencyName { get; set; }
        public string ConvertedTokenAmount { get; set; }

        public long? NoOfUnitsCount { get; set; }
        public string LeadStatus { get; set; }
        public long? PropertyConsultantId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? ProjectDetailId { get; set; }
        [Display(Name= "Consultant Name")]
        public string PropertyConsultantName { get; set; }
        public long? DirectReporteeId { get; set; }
        [Display(Name = "Direct Reportee Name")]
        public string DirectReporteeName { get; set; }

        public long? PositionId { get; set; }

        [Required]
        [Display(Name = "Payment Plan Name")]
        public long PaymentPlanId { get; set; }
        [Display(Name = "Payment Plan Name")]
        public string PaymentPlanName { get; set; }
        [Display(Name = "Discount In %")]
        public float? PaymentPlanDiscount { get; set; }
        [Display(Name = "Discount Amount")]
        public string DiscountAmount { get; set; }
        public string Bank { get; set; }
        public string SalesAgent { get; set; }
        public bool IsPropertyConsultant { get; set; }
        public string PCCommission { get; set; }
       
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Sale Date")]
        public DateTime? SalesDate { get; set; }
        public string SalesAmount { get; set; }
        public string DPAmount { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "DP Date")]
        public DateTime? DPDate { get; set; }

    }
}

