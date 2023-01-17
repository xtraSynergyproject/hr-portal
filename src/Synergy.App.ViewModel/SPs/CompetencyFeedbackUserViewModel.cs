using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class CompetencyFeedbackUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoName { get; set; }
        public string CompetencyName { get; set; }
        public decimal Rating { get; set; }
        public long SelfCount { get; set; }
        public long OhersCount { get; set; }
    }
}
