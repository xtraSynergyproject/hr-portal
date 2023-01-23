using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class JDRequestViewModel : BaseViewModel
    {
  
        public virtual int TransactionId { get; set; }
        public virtual bool IsApprovalRequired { get; set; }
        public virtual string Model { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }

    }
}
