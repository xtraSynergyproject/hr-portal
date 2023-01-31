using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ProjectDashboardChartViewModel : ViewModelBase
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public string Type1 { get; set; }
        public double Count1 { get; set; }
        public double Count2 { get; set; }

        public long? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public long? AssigneeId { get; set; }
        public string TaskCreatedDate { get; set; }
       
    }
}
