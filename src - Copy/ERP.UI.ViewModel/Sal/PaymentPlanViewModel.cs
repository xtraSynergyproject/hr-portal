using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PaymentPlanViewModel: ViewModelBase
    {
        //14-10-2019 New Payment Plan
        [Display(Name = "Payment Plan")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Payment Plan Description")]
        public string Description { get; set; }
        public string Plan { get; set; }
        [Display(Name = "Previous Plan")]
        public string PrevPlan { get; set; }
        [Display(Name = "Discount (%)")]
        public float? Discount { get; set; }
        [Display(Name = "On Booking (%)")]
        public float? OnBooking { get; set; }
        [Display(Name = "In 6 Months (%)")]
        public float? In6Months { get; set; }
        [Display(Name = "In 12 Months (%)")]
        public float? In12Months { get; set; }
        [Display(Name = "At Handover (%)")]
        public float? AtHandover { get; set; }
        [Display(Name = "After 1 Year (%)")]
        public float? Afteryear1 { get; set; }
        [Display(Name = "After 2 Year (%)")]
        public float? Afteryear2 { get; set; }
        [Display(Name = "After 3 Year (%)")]
        public float? Afteryear3 { get; set; }
        [Display(Name = "After 4 Year (%)")]
        public float? Afteryear4 { get; set; }
        [Display(Name = "After 5 Year (%)")]
        public float? Afteryear5 { get; set; }
        ////

        //  [Display(Name = "Project Name")]
        //  public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Project")]
        public long ProjectId { get; set; }
        //  [Display(Name = "Payment Plan")]
        //  [Required]
        //  public string Name { get; set; }
        //  //[Required]
        //  //[Display(Name = "Unit Price")]
        ////  public string UnitPrice { get; set; }
        //  [Display(Name = "Unit Type")]
        //  public string UnitTypeName { get; set; }
        //  public long UnitType { get; set; }
        //  [Display(Name = "DLD Fee")]
        //  public string DLDFee { get; set; }
        //  [Display(Name = "Okoud Fee")]
        //  public string OkoudFee { get; set; }
        //  public string VAT { get; set; }
        //  [Display(Name = "Total Cost")]
        //  public string TotalCost { get; set; }
        //  [Display(Name = "Down Pay")]
        //  public string DownPayment { get; set; }        
        //  [Display(Name = "Pay Mode")]
        //  public string PayMode { get; set; }
        //  [Display(Name = "Pay Status")]
        //  public string PayStatus{ get; set; }

    }
}
