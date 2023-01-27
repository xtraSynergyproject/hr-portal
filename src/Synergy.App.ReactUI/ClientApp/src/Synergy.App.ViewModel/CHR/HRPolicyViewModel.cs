using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class HRPolicyViewModel
    {
        public string PolicyName { get; set; }
        public string PolicyDescription { get; set; }
        public string PolicyDocument { get; set; }
        public DateTime StartDate { get; set; }
        public string NtsNoteId { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
