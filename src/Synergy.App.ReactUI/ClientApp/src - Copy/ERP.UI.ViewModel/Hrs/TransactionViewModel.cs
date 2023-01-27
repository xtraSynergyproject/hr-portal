using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TransactionViewModel : BaseViewModel
    {
        public virtual int Id { get; set; }

        public virtual string TransactionType { get; set; }

        public virtual string Owner { get; set; }

        public virtual TransactionStatusEnum TransactionStatus { get; set; }

        public virtual string TransactionProcessStatus { get; set; }

        public virtual RequestStatusEnum RequestStatus { get; set; }

        public virtual string ApprovedBy { get; set; }

        public virtual Nullable<System.DateTime> ApprovedOn { get; set; }

        public virtual string SynergyReferenceId { get; set; }

        public virtual string FusionReferenceId { get; set; }

        public virtual Nullable<int> TaskId { get; set; }

        public virtual string Description { get; set; }
    }
}
