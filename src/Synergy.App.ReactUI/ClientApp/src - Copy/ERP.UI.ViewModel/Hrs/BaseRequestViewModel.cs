using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class BaseRequestViewModel : BaseViewModel
    {

        //public virtual TransactionViewModel Transaction { get; set; }

        public virtual TransactionStatusEnum TransactionStatus { get; set; }

        public virtual TransactionProcessStatusEnum TransactionProcessStatus { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual string Owner { get; set; }

        public virtual RequestStatusEnum RequestStatus { get; set; }

        public virtual int TransactionId { get; set; }
        public virtual bool IsApprovalRequired { get; set; }
        public virtual string Model { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime RequestedDate { get; set; }


        // public virtual DataLoadMode Mode { get; set; }


    }
}
