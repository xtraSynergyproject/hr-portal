using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_Misconduct : NodeBase
    {
        public long EmployeeId { get; set; }
        public string EmployeeStatus { get; set; }
        public string PositionOrJob { get; set; }
        public string EmployeeName { get; set; }
        public DateTime HireDate { get; set; }
        public string Organization { get; set; }
        public string Code { get; set; }
        public string Misconduct { get; set; }
        public DateTime Date { get; set; }
        public string MisconductStatus { get; set; }
        public String Action { get; set; }
        public string FineAmount { get; set; }
        public string Descrption { get; set; }
        public string InvestigationResult { get; set; }
        public string MisconductType { get; set; }
        public long Approved { get; set; }
    }
}
    