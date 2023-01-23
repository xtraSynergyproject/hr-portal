using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class MisconductViewModel
    {
        public string Id { get; set; }
        public string ServiceNo { get; set; }
        public string MisconductTypeName { get; set; }
        public string DisciplinaryActionTakenName { get; set; }
        public DateTime? MisconductDate { get; set; }
        public string ServiceId { get; set; }

       public string UserId { get; set; }
        public string Status { get; set; }
        //public string Id { get; set; }
        //public string EmployeeName { get; set; }
    }
}