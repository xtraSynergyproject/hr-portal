using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_FixedAllowances : NodeBase
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public float BasicSalary { get; set; }
        public float HousingAllowanceInPercentage { get; set; }
        public float TransportationAllowanceInPercentage { get; set; }
        public float HousingAllowanceAmount { get; set; }
        public float TransportationAllowAmount { get; set; }
        public float TicketAllowance { get; set; }
        public float GOSIEmployee { get; set; }
        public float GOSICompany { get; set; }
        public float TotalEarning { get; set; }
        public float TotalDeductions { get; set; }
        public float NetPayroll { get; set; }

    }
}
