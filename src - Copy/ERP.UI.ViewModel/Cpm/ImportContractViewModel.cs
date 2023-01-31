using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{  
    public class ImportContractViewModel : ViewModelBase
    {
        public string ProjectName { get; set; }
        public string UnitNo { get; set; }
        public string TenantName { get; set; }
        public string TenantEmail { get; set; }
        public string ContractNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalRent { get; set; }
        public string ContractPaymentType { get; set; }
        public string MoneyHeldBy { get; set; }
        public string DemoghraphicType { get; set; }
        public string ContractStatus { get; set; }
        public string Deposit { get; set; }
        public string ErrorMessage { get; set; }        
        public long ProjectId { get; set; }
        public long UnitId { get; set; }
        public long TenantId { get; set; }
        public long UserId { get; set; }

    }
}
