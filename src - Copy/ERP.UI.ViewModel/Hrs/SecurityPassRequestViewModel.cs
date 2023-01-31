using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SecurityPassRequestViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public virtual int EmployeeId { get; set; }

        public int TransactionId { get; set; }

        public string Errors { get; set; }

        [Required]
        [Display(Name = "Pass Number")]
        public virtual string PassNumber { get; set; }
        [Required]
        [Display(Name = "Issue Date")]
        public virtual System.DateTime? IssueDate { get; set; }
        [Required]
        [Display(Name = "Expiry Date")]
        public virtual System.DateTime? ExpiryDate { get; set; }
        [Required]
        [Display(Name = "Pass Holder Id")]
        public virtual string PassHolderId { get; set; }
        public virtual string Area { get; set; }

        public virtual string Department { get; set; }
        public virtual string StaffNumber { get; set; }
        public virtual string ProcessType { get; set; }
        public virtual string Fees { get; set; }
        public virtual string Weightage { get; set; }

    }
}
