using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class HelpDeskViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double InProgress { get; set; }
        public double Draft { get; set; }
        public double OverDue { get; set; }
        public double Completed { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateName { get; set; }       
        public string Priority { get; set; }
        public string CategoryCode { get; set; }
        
        public double Count { get; set; }
        public string Day { get; set; }
        public double Violated { get; set; }
        public double NonViolated { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
